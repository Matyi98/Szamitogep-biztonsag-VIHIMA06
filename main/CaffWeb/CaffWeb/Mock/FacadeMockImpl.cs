using CaffDal.Domain;
using CaffDal.Domain.Pager;
using CaffDal.Services;

namespace CaffWeb.Mock
{
    public class FacadeMockImpl : ICaffFacade
    {

        class Comment {
            public int id { get; set; }
            public int caffId { get; set; }
            public string comment { get; set; }
            public int uploader { get; set; }
        }

        static List<UploadRequest> uploads = new List<UploadRequest> {
            new UploadRequest { CaffName = "MockCaff1", OwnerId=1, RawCaff=null },
            new UploadRequest { CaffName = "MockCaff2", OwnerId=1, RawCaff=null },
        };
        static List<Comment> comments = new List<Comment> {
            new Comment { id = 0, caffId=0, comment="Ejj de szép!", uploader=1 },
            new Comment { id = 1, caffId=0, comment="Hűha, az igen!", uploader=1 },
            new Comment { id = 2, caffId=1, comment="Nem tetszik.", uploader=1 },
        };

        public async Task<DownloadRequest> BuyCaff(int caffId) {
            return new DownloadRequest {
                Bytes = new byte[1024],
                Name = uploads[caffId].CaffName,
            };
        }

        public async Task DeleteCaff(int caffId) {
            uploads.RemoveAt(caffId);
        }

        public async Task DeleteComment(int commentId) {
            comments.RemoveAt(commentId);
        }

        public async Task<IReadOnlyCollection<CommentResponse>> GetComments(int caffId) {
            return comments.Where(y=>y.caffId == caffId).Select(x => new CommentResponse {
                Id = x.id,
                CommenterId = x.uploader,
                Commenter = "x.uploader.name",
                Text = x.comment,
                CreationDate = DateTime.Now,
            }).ToList();
        }

        public async Task<DetailedPreviewResponse> GetPreview(int caffId) {
            var data = uploads[caffId];
            return new DetailedPreviewResponse {
                Name = data.CaffName,
                CaffID = caffId,
                CreatorID = data.OwnerId,
                CreatorDate = DateTime.Now,
                Creator = "data.OwnerId",
                ImageMetas = new List<ImageMetaResponse> {
                    new ImageMetaResponse {
                        Delay = 1,
                        Id = 0                    
                    },
                    new ImageMetaResponse {
                        Delay = 4,
                        Id = 1
                    },
                    new ImageMetaResponse {
                        Delay = 1,
                        Id = 2
                    },
                }
            };
        }

        public async Task ModifyComment(int commentId, string comment) {
            comments[commentId].comment = comment;
        }

        public async Task<PagedResult<CompactPreviewResponse>> PagedSearch(PagedSearchSpecification specification) {
            return new PagedResult<CompactPreviewResponse> {
                PageNumber = 1,
                PageSize = 4,
                TotalCount = 10,
                Results = new List<CompactPreviewResponse> {
                    new CompactPreviewResponse {
                        Id = 0,
                        CreationDate = DateTime.Now,
                        Creator = "aLMA",
                        ImageId = 0,
                        Name = "CAffDummy1"
                    },
                    new CompactPreviewResponse {
                        Id = 1,
                        CreationDate = DateTime.Now,
                        Creator = "aLMA",
                        ImageId = 0,
                        Name = "CAffDummy1"
                    },
                    new CompactPreviewResponse {
                        Id = 2,
                        CreationDate = DateTime.Now,
                        Creator = "aLMA",
                        ImageId = 0,
                        Name = "CAffDummy1"
                    },
                    new CompactPreviewResponse {
                        Id = 3,
                        CreationDate = DateTime.Now,
                        Creator = "aLMA",
                        ImageId = 0,
                        Name = "CAffDummy1"
                    },
                }
            };
        }

        public async Task<UploadResponse> UploadCaff(UploadRequest request) {
            uploads.Add(request);
            return new UploadResponse { CaffId = uploads.Count-1 };
        }

        public async Task WriteComment(int caffId, string comment, int uploader) {
            var id = comments.Count;
            comments.Add(new Comment {
                id = id,
                caffId = caffId,
                comment = comment,
                uploader = uploader
            });
        }

        public async Task<CommentResponse> GetCommentById(int commentId) {
            var x =  comments[commentId];
            return new CommentResponse {
                Id = commentId,
                CommenterId = x.uploader,
                Commenter = "x.uploader.name",
                Text = x.comment,
                CreationDate = DateTime.Now,
            };
        }

        public Task<byte[]> GetImage(int imageId) {
            return null;
        }
    }
}
