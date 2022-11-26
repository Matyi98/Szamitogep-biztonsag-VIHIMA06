using CaffDal.Domain;
using CaffDal.Domain.Pager;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task<UploadResponse> UploadCaff(UploadRequest request)
        {
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

            throw new NotImplementedException();
        }

        public Task WriteComment(int caffId, string comment, int uploader)
        {
            throw new NotImplementedException();
        }
    }
}
