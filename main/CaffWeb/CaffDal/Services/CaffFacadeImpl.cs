using CaffDal.Domain;
using CaffDal.Domain.Pager;
using Microsoft.EntityFrameworkCore;
using CliWrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using CaffDal.Entities;
using System.ComponentModel.Design;
using CaffDal.ParserWrapper;
using Microsoft.Extensions.Options;
using System.IO;

namespace CaffDal.Services
{
    public class CaffFacadeImpl : ICaffFacade
    {
        private readonly CaffDbContext _context;
        private readonly CaffParserConfig _parserConfig;
        public CaffFacadeImpl(CaffDbContext context, IOptions<CaffParserConfig> config)
        {
            _context = context;
            _parserConfig = config.Value;
        }

        public Task<DownloadRequest> BuyCaff(int caffId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCaff(int caffId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteComment(int commentId)
        {
            throw new NotImplementedException();
        }

        public async Task<CommentResponse> GetCommentById(int commentId)
        {
            var comment = await _context
                .Comments
                .Include(c => c.User)
                .SingleOrDefaultAsync(c => c.Id == commentId);

            // TODO: Copy this to the domain objects or use automapper
            CommentResponse response = new CommentResponse()
            {
                Id = comment.Id,
                CommenterId = comment.UserId,
                Commenter = comment.User.CustomName,
                CreationDate = comment.CreationDate,
                Text = comment.Text
            };

            return response;
        }

        public Task<IReadOnlyCollection<CommentResponse>> GetComments(int caffId)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetImage(int imageId)
        {
            throw new NotImplementedException();
        }

        public Task<DetailedPreviewResponse> GetPreview(int caffId)
        {
            throw new NotImplementedException();
        }

        public Task ModifyComment(int commentId, string comment)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<CompactPreviewResponse>> PagedSearch(PagedSearchSpecification specification)
        {
            throw new NotImplementedException();
        }

        public async Task<UploadResponse> UploadCaff(UploadRequest request)
        {

            /*
             * Execute exe and read manifest
             * */
            List<Image> CiffList;
            Caff caff;
            try
            { 
            var tempFileName = Guid.NewGuid().ToString();
            File.WriteAllBytes(_parserConfig.OutputWorkdir + tempFileName, request.RawCaff);

            await Cli.Wrap(_parserConfig.ParserPath)
                .WithArguments(_parserConfig.OutputWorkdir + tempFileName + " " + _parserConfig.OutputWorkdir)
                .ExecuteAsync();
            string[] lines = File.ReadLines(_parserConfig.OutputWorkdir + "manifest").ToArray();

            /*
             * Create Caff
             * */
            caff = new Caff(creator: lines[0].Split(": ")[1], request.RawCaff);
            caff.CreatorDate = CiffDateToDateAndTime(lines[1].Split(": ")[1]);
            caff.NumberOfFrames = Convert.ToInt32(lines[2].Split(": ")[1]);
            caff.UserId = request.OwnerId;
            caff.CaffName = request.CaffName;
            CiffList = StringArrayToCiffList(3, lines);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //Erase everything from temp directory
                DirectoryInfo di = new DirectoryInfo(_parserConfig.OutputWorkdir);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }

            /*
             * Add ciff's to caff
             * */
            foreach (Image ciff in CiffList)
            {
                caff.Images.Add(ciff);
            }
            //Just for testing purpose:
            //File.WriteAllBytes("Happy.jpg", caff.Images.First().Preview);

            /*
             * Save to db
             * */
            _context.Add(caff);
            await _context.SaveChangesAsync();

            return new UploadResponse{ CaffId = caff.Id };

            /*
               Halott Pénz: Caffatokra törted a szívem
               szánd meg hát szomorú szívem,
               Úgysincs más vigaszom nekem,
               Jöjj el hát, jöjj el hát,
               Hogy egy összetört szívet megragassz!
               Szánd meg, caffatokra összetört szívem
               Szánd meg hát szomorú szívem,
               Úgysincs más vigaszom nekem,
            */
        }
 
        private DateTime CiffDateToDateAndTime(string ciffDate)
        {
            string[] uglyDateArray = ciffDate.Split(' ');
            string[] ymd = uglyDateArray[0].Split('.');
            string[] hs = uglyDateArray[1].Split(':');
            return new DateTime(year: Convert.ToInt32(ymd[0]), month: Convert.ToInt32(ymd[1]), day: Convert.ToInt32(ymd[2]),
                hour: Convert.ToInt32(hs[0]), minute: Convert.ToInt32(hs[1]), second: 0);
        }

        private List<Image> StringArrayToCiffList(int readFrom, String[] lines)
        {
            List<Image> CiffList = new List<Image>();
            for (int i = readFrom; i < lines.Length; i += 5)
            {
                var ciff = new Image(File.ReadAllBytes(_parserConfig.OutputWorkdir + lines[i]));
                ciff.Duration = Convert.ToInt32(lines[i + 1].Split(':')[1]);
                ciff.Caption = lines[i + 2].Split(':')[1];
                ciff.Tags = lines[i + 3].Split(':')[1].Split(';').ToList<string>();
                ciff.Width = Convert.ToInt32(lines[i + 4].Split(':')[1].Split('*')[0]);
                ciff.Height = Convert.ToInt32(lines[i + 4].Split(':')[1].Split('*')[1]);
                CiffList.Add(ciff);
            }
            return CiffList;
        }

        public Task WriteComment(int caffId, string comment, int uploader)
        {
            throw new NotImplementedException();
        }
    }
}
