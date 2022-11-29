using CaffDal.Domain;
using CaffDal.Domain.Pager;
using Microsoft.EntityFrameworkCore;
using CliWrap;
using CaffDal.Entities;
using CaffDal.ParserWrapper;
using Microsoft.Extensions.Options;
using CaffDal.Exceptions;

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

        public async Task<DownloadRequest> BuyCaff(int caffId)
        {
            var caff = await _context
                .Caffs
                .SingleOrDefaultAsync(caff => caff.Id == caffId)
                ?? throw new EntityNotFoundException($"Caff doesn't exists with id {caffId}!");

            DownloadRequest request = new DownloadRequest()
            {
                Bytes = caff.RawCaff,
                Name = caff.CaffName // This was caff.Creator, but I think this should be caff.CaffName
            };

            return request;
        }

        public async Task DeleteCaff(int caffId)
        {
            var caff = await _context
                .Caffs
                .SingleOrDefaultAsync(caff => caff.Id == caffId)
                ?? throw new EntityNotFoundException($"Caff doesn't exists with id {caffId}!");

            _context.Caffs.Remove(caff);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteComment(int commentId)
        {
            var comment = await _context
                .Comments
                .SingleOrDefaultAsync(comment => comment.Id == commentId)
                ?? throw new EntityNotFoundException($"Comment doesn't exists with id {commentId}!");

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<CommentResponse> GetCommentById(int commentId)
        {
            var comment = await _context
                .Comments
                .Include(c => c.User)
                .SingleOrDefaultAsync(c => c.Id == commentId)
                ?? throw new EntityNotFoundException($"Comment doesn't exists with id {commentId}!");

            CommentResponse response = new CommentResponse()
            {
                Id = comment.Id,
                CommenterId = comment.UserId ?? -1, // TODO: Maybe exclude comments, when its user is deleted, but this shouldn't be possible anyway
                Commenter = comment.User != null ? comment.User.CustomName : "DeletedUser",
                CreationDate = comment.CreationDate,
                Text = comment.Text
            };

            return response;
        }

        public async Task<IReadOnlyCollection<CommentResponse>> GetComments(int caffId)
        {
            var comments = await _context
                .Comments
                .Include(comment => comment.User)
                .Where(comment => comment.CaffId == caffId)
                .ToListAsync(); // TODO: Sort comments by date?

            List<CommentResponse> response = new List<CommentResponse>();

            foreach (var comment in comments)
            {
                CommentResponse commentResponse = new CommentResponse()
                {
                    Id = comment.Id,
                    CommenterId = comment.UserId ?? -1,
                    Commenter = comment.User != null ? comment.User.CustomName : "DeletedUser",
                    CreationDate = comment.CreationDate,
                    Text = comment.Text
                };
                response.Add(commentResponse);
            }

            return response;
        }

        public async Task<byte[]> GetImage(int imageId)
        {
            var image = await _context
                .Images
                .SingleOrDefaultAsync(image => image.Id == imageId)
                ?? throw new EntityNotFoundException($"Image doesn't exists with id {imageId}!");

            return image.Preview;
        }

        public async Task<DetailedPreviewResponse> GetPreview(int caffId)
        {
            var caff = await _context.Caffs
                    .Include(c => c.Images)
                    .SingleOrDefaultAsync(c => c.Id == caffId)
                    ?? throw new EntityNotFoundException($"Caff doesn't exists with id {caffId}!");

            List<ImageMetaResponse> imageMetaList = new List<ImageMetaResponse>();
            foreach(var image in caff.Images)
            {
                imageMetaList.Add(new ImageMetaResponse { Id = image.Id, Delay = image.Duration });
            }

            var detailedPreviewResponse = new DetailedPreviewResponse
            {
                Name = caff.CaffName,
                Creator = caff.Creator,
                CreatorDate = caff.CreatorDate,
                CaffID = caff.Id,
                CreatorID = caff.UserId,
                ImageMetas = imageMetaList
            };

            return detailedPreviewResponse;
        }

        public async Task ModifyComment(int commentId, string comment)
        {
            var c = await _context
                .Comments
                .SingleOrDefaultAsync(comment => comment.Id == commentId)
                ?? throw new EntityNotFoundException($"Comment doesn't exists with id {commentId}!");

            c.Text = comment;
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<CompactPreviewResponse>> PagedSearch(PagedSearchSpecification specification)
        {
            var filteredCaffs = await _context.Caffs
                .Where(c => (specification.CreationDateStart != null || c.CreatorDate >= specification.CreationDateStart) &&
                            (specification.CreationDateStart != null || c.CreatorDate <= specification.CreationDateEnd) &&
                            (specification.Name != null || c.CaffName == specification.Name) &&
                            (specification.Creator != null || c.Creator == specification.Creator))
                .Include(c => c.Images) // TODO: Is this necessary? Maybe we can use getimage function for faster response time.
                .ToListAsync();  // TODO: Do we need any sorting?

            List<CompactPreviewResponse> previews = new List<CompactPreviewResponse>();
            var startIndex = specification.PageNumber * specification.PageSize;
            foreach(var caff in filteredCaffs.GetRange(startIndex, startIndex + specification.PageSize - 1))
            {
                previews.Add(new CompactPreviewResponse
                {
                    Id = caff.Id,
                    CreationDate = caff.CreatorDate,
                    Creator = caff.Creator,
                    ImageId = caff.Images.First().Id,
                    Name = caff.CaffName
                });
            }

            PagedResult<CompactPreviewResponse> result = new PagedResult<CompactPreviewResponse>()
            {
                PageNumber = specification.PageNumber,
                PageSize = specification.PageSize,
                TotalCount = filteredCaffs.Count,
                Results = previews
            };
            return result;
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
                /*
                 * Erase everything from temp directory
                 */
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
            _context.Caffs.Add(caff);
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
                //ciff.Tags = lines[i + 3].Split(':')[1].Split(';').ToList<string>();
                ciff.Width = Convert.ToInt32(lines[i + 4].Split(':')[1].Split('*')[0]);
                ciff.Height = Convert.ToInt32(lines[i + 4].Split(':')[1].Split('*')[1]);
                CiffList.Add(ciff);
            }
            return CiffList;
        }

        public async Task WriteComment(int caffId, string comment, int uploader)
        {
            var efComment = new Comment(comment)
            {
                CaffId = caffId,
                UserId = uploader,
                CreationDate = DateTime.Now
            };

            _context.Comments.Add(efComment);
            await _context.SaveChangesAsync();
        }
    }
}
