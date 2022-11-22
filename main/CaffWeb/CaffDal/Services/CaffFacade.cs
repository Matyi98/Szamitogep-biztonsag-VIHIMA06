using CaffDal.Domain;
using CaffDal.Domain.Pager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaffDal.Services
{
    public interface CaffFacade
    {
        public Task<PagedResult<CompactPreviewResponse>> PagedSearch(PagedSearchSpecification specification);
        public Task<DetailedPreviewResponse> GetPreview(int caffId);
        public Task<DownloadRequest> BuyCaff(int caffId);
        public Task<UploadResponse> UploadCaff(UploadRequest request);
        public Task DeleteCaff(int caffId);


        public Task WriteComment(int caffId, string comment, int uploader);
        public Task ModifyComment(int commentId, string comment);
        public Task DeleteComment(int commentId);
        public Task<IReadOnlyCollection<CommentResponse>> GetComments(int caffId);
        public Task<CommentResponse> GetCommentById(int commentId);
    }
}
