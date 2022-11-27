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
using CaffDal.Services.Parser;
using CaffDal.Entities;

namespace CaffDal.Services
{
    public class CaffFacadeImpl : ICaffFacade
    {
        private readonly CaffDbContext _context;

        public CaffFacadeImpl(CaffDbContext context)
        {
            _context = context;
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
            await Cli.Wrap("native_parser.exe")
                .WithArguments(request.CaffName)
                .ExecuteAsync();
            string[] lines = File.ReadLines("manifest").ToArray();

            /*
             * Create Caff
             * */
            Caff caff = new Caff(lines[0].Split(':')[1], request.RawCaff);
            caff.CreatorDate = CiffDateToDateAndTime(lines[1].Split(':')[1]);
            caff.NumberOfFrames = Convert.ToInt32(lines[2].Split(':')[1]);
            caff.Id = new Random().Next();
            caff.UserId = request.OwnerId;
            //caff.User = ;
            //caff.Creator = ;
            List<Ciff> CiffList = StringArrayToCiffList(3, lines);

            /*
             * Add ciff's to caff
             * */
            foreach (Ciff ciff in CiffList)
            {
                Image image = new(Parser.Parser.display(ciff));
                image.Caff = caff;
                image.Duration = ciff.Duration;
                image.Preview = Parser.Parser.display(ciff);
                image.CaffId = caff.Id;
                caff.Images.Add(image);
            }
            /*
             * Todo save to db
             * */

            return new UploadResponse{ CaffId = caff.Id };

            // TODO:
            // 1. Call C++ exe (with RawCaff, caffName)
            // 2. Read manifest file
            // 3. Read Ciff files
            // 4. Save manifest data and the rawcaff (from the uploadRequest) to the Caff table.
            // 5. Save changes?
            // 6. Convert rawCiff to picture? (call python code here maybe)
            // 7. Save Ciff data to the Image table.
            // 8. Save changes!
            // 9. Create and return UploadResponse.
        }

        private DateTime CiffDateToDateAndTime(string ciffDate)
        {
            string[] uglyDateArray = ciffDate.Split(' ');
            string[] ymd = uglyDateArray[0].Split('.');
            string[] hs = uglyDateArray[0].Split(':');
            return new DateTime(year: Convert.ToInt32(ymd[0]), month: Convert.ToInt32(ymd[1]), day: Convert.ToInt32(ymd[2]),
                hour: Convert.ToInt32(hs[0]), minute: Convert.ToInt32(hs[1]), second: 0);
        }

        private List<Ciff> StringArrayToCiffList(int readFrom, String[] lines)
        {
            List<Ciff> CiffList = new List<Ciff>();
            for (int i = readFrom; i < lines.Length; i += 6)
            {
                var ciff = new Parser.Ciff(File.ReadAllBytes(lines[i]));
                ciff.Duration = Convert.ToInt32(lines[i + 1].Split(':')[1]);
                ciff.Caption = lines[i + 2].Split(':')[1];
                ciff.Tags = lines[i + 3].Split(':')[1].Split(';').ToList<string>();
                ciff.Width = Convert.ToInt32(lines[i + 4].Split(':')[1].Split('*')[0]);
                ciff.Height = Convert.ToInt32(lines[i + 5].Split(':')[1].Split('*')[1]);
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
