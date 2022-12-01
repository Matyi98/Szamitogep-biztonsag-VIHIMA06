using CaffDal.Domain;
using CaffDal.Domain.Pager;
using CaffDal.Entities;
using CaffDal.Exceptions;
using CliWrap;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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

            DownloadRequest request = new()
            {
                Bytes = caff.RawCaff,
                Name = caff.CaffName
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

            CommentResponse response = new()
            {
                Id = comment.Id,
                CommenterId = comment.UserId ?? -1,
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
                .ToListAsync();

            List<CommentResponse> response = new();

            foreach (var comment in comments)
            {
                CommentResponse commentResponse = new() {
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
                    .Select(c => new
                    {
                        ImageObjects = c.Images.Select(i => new { i.Id, i.Duration }),
                        Id = c.Id,
                        CreatorDate = c.CreatorDate,
                        Creator = c.Creator,
                        Name = c.CaffName,
                        CreatorID = c.UserId
                    })
                    .SingleOrDefaultAsync(c => c.Id == caffId)
                    ?? throw new EntityNotFoundException($"Caff doesn't exists with id {caffId}!");

            List<ImageMetaResponse> imageMetaList = new();
            foreach (var image in caff.ImageObjects)
            {
                imageMetaList.Add(new ImageMetaResponse { Id = image.Id, Delay = image.Duration });
            }

            var detailedPreviewResponse = new DetailedPreviewResponse
            {
                Name = caff.Name,
                Creator = caff.Creator,
                CreatorDate = caff.CreatorDate,
                CaffID = caff.Id,
                CreatorID = caff.CreatorID,
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
            if (specification.PageSize <= 0)
                specification.PageSize = 4;

            if (specification.PageNumber <= 0)
                specification.PageNumber = 1;

            var startIndex = (specification.PageNumber - 1) * specification.PageSize;

            var filteredCaffs = await _context.Caffs
                .Where(c => (specification.CreationDateStart == null || c.CreatorDate >= specification.CreationDateStart) &&
                            (specification.CreationDateEnd == null || c.CreatorDate <= specification.CreationDateEnd) &&
                            (specification.Name == null || c.CaffName.Contains(specification.Name)) &&
                            (specification.Creator == null || c.Creator.Contains(specification.Creator)))
                .Include(c => c.Images)
                .Select(c => new CompactPreviewResponse
                {

                    ImageId = c.Images.Select(i => i.Id).First(),
                    Id = c.Id,
                    CreationDate = c.CreatorDate,
                    Creator = c.Creator,
                    Name = c.CaffName
                })
                .ToListAsync();

            var pageFilteredCaffs = filteredCaffs.Skip(startIndex).Take(specification.PageSize).ToList();

            PagedResult<CompactPreviewResponse> result = new()
            {
                PageNumber = specification.PageNumber,
                PageSize = specification.PageSize,
                TotalCount = filteredCaffs.Count,
                Results = pageFilteredCaffs
            };
            return result;
        }

        public async Task<UploadResponse> UploadCaff(UploadRequest request)
        {
            var tempFileName = "caffFile";
            var tempFolderName = Guid.NewGuid().ToString();
            var workDir = _parserConfig.OutputWorkdir + tempFolderName + "/";
            if (!Directory.Exists(_parserConfig.OutputWorkdir))
            {
                Directory.CreateDirectory(_parserConfig.OutputWorkdir);
            }
            Directory.CreateDirectory(workDir);
            List<Image> CiffList;
            Caff caff;
            try
            {
                File.WriteAllBytes(workDir + tempFileName, request.RawCaff);

                await Cli.Wrap(_parserConfig.ParserPath)
                    .WithArguments(workDir + tempFileName + " " + workDir)
                    .ExecuteAsync();
                string[] lines = File.ReadLines(workDir + "manifest").ToArray();
                caff = new Caff(creator: lines[0].Split(": ")[1], request.RawCaff) {
                    CreatorDate = CiffDateToDateAndTime(lines[1].Split(": ")[1]),
                    NumberOfFrames = Convert.ToInt32(lines[2].Split(": ")[1]),
                    UserId = request.OwnerId,
                    CaffName = request.CaffName
                };
                CiffList = StringArrayToCiffList(3, lines, workDir);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DirectoryInfo di = new(workDir);
                di.Delete(true);
            }
            foreach (Image ciff in CiffList)
            {
                ciff.Preview = Converter.Convert(ciff, _parserConfig.ImageQuality);
                caff.Images.Add(ciff);
            }
            _context.Caffs.Add(caff);
            await _context.SaveChangesAsync();

            return new UploadResponse { CaffId = caff.Id };
        }

        private static DateTime CiffDateToDateAndTime(string ciffDate)
        {
            string[] uglyDateArray = ciffDate.Split(' ');
            string[] ymd = uglyDateArray[0].Split('.');
            string[] hs = uglyDateArray[1].Split(':');
            return new DateTime(year: Convert.ToInt32(ymd[0]), month: Convert.ToInt32(ymd[1]), day: Convert.ToInt32(ymd[2]),
                hour: Convert.ToInt32(hs[0]), minute: Convert.ToInt32(hs[1]), second: 0);
        }

        private static List<Image> StringArrayToCiffList(int readFrom, String[] lines, string workDir)
        {
            List<Image> CiffList = new();
            for (int i = readFrom; i < lines.Length; i += 5)
            {
                var ciff = new Image(File.ReadAllBytes(workDir + lines[i])) {
                    Duration = Convert.ToInt32(lines[i + 1].Split(':')[1]),
                    Caption = lines[i + 2].Split(':')[1],
                    Width = Convert.ToInt32(lines[i + 4].Split(':')[1].Split('*')[0]),
                    Height = Convert.ToInt32(lines[i + 4].Split(':')[1].Split('*')[1])
                };
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

        public async Task DeleteUser(int userId)
        {
            var user = await _context.Users.Include(x => x.Comments).SingleOrDefaultAsync(u => u.Id == userId)
                ?? throw new EntityNotFoundException();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
