using CaffDal;
using CaffDal.Domain;
using CaffDal.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Tests
{
    public class IntegrationTests
    {
        CaffFacadeImpl facade;
        CaffDbContext context;
        UploadRequest request;
        string baseDir = "../../../../";

        public IntegrationTests()
        {
            context = buildContext();
            facade = buildFacade();
            request = generateRequest();
        }

        private CaffDbContext buildContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CaffDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            return new CaffDbContext(optionsBuilder.Options);
        }

        private CaffFacadeImpl buildFacade()
        {
            var config = new CaffParserConfig();
            config.ImageQuality = 20;
            config.ParserPath = baseDir + "CaffWeb/libs/native_parser.exe";
            config.OutputWorkdir = baseDir + "CaffWeb/out/";

            var options = Options.Create(config);
            var logger = Mock.Of<ILogger<CaffFacadeImpl>>();
            return new CaffFacadeImpl(context, options, logger);
        }

        private UploadRequest generateRequest()
        {
            request = new UploadRequest();
            request.RawCaff = File.ReadAllBytes(baseDir + "Tests/1.caff");
            request.CaffName = "test";
            request.OwnerId = 1;
            return request;
        }

        [Fact]
        public async void test_caff_upload()
        {
            var result = await facade.UploadCaff(request);
            Assert.Single(context.Caffs);
            await facade.DeleteCaff(result.CaffId);
        }

        [Fact]
        public async void test_caff_deletion()
        {
            var result = await facade.UploadCaff(request);
            await facade.DeleteCaff(result.CaffId);
            Assert.Empty(context.Caffs);
        }

        [Fact]
        public async void test_caff_purchase()
        {
            var result = await facade.UploadCaff(request);
            var buyResult = await facade.BuyCaff(result.CaffId);
            Assert.True(buyResult.Name== request.CaffName && buyResult.Bytes == request.RawCaff);
            await facade.DeleteCaff(result.CaffId);
        }

        [Fact]
        public async void test_write_comment()
        {
            await facade.WriteComment(1, "test comment", 1);
            Assert.Single(context.Comments);
            await facade.DeleteComment(context.Comments.First(e => e.Text=="test comment").Id);
        }

        [Fact]
        public async void test_delete_comment()
        {
            await facade.WriteComment(1, "test comment", 1);
            await facade.DeleteComment(context.Comments.First(e => e.Text == "test comment").Id);
            Assert.Empty(context.Comments);
        }

        [Fact]
        public async void test_get_comment_by_id()
        {
            await facade.WriteComment(1, "test comment", 1);
            var comment = context.Comments.First(e => e.Text == "test comment");
            var result = await facade.GetCommentById(comment.Id);
            Assert.True(result.Text == comment.Text);
            await facade.DeleteComment(context.Comments.First(e => e.Text == "test comment").Id);
        }

        [Fact]
        public async void test_get_comments()
        {
            var caff = await facade.UploadCaff(request);
            await facade.WriteComment(caff.CaffId, "test comment", 1);
            var result = await facade.GetComments(caff.CaffId);
            Assert.Single(result);
            await facade.DeleteComment(context.Comments.First(e => e.Text == "test comment").Id);
            await facade.DeleteCaff(caff.CaffId);
        }

        [Fact]
        public async void test_modify_comment()
        {
            var caff = await facade.UploadCaff(request);

            await facade.WriteComment(caff.CaffId, "test comment", 1);
            var oldComment = context.Comments.First(e => e.Text == "test comment");

            await facade.ModifyComment(oldComment.Id, "new test comment");
            var newComment = context.Comments.First(e => e.Text == "new test comment");

            Assert.Single(context.Comments.Where(e => e.Id == newComment.Id));
            Assert.Single(context.Comments.Where(e => e.Text == "new test comment"));
            Assert.Empty(context.Comments.Where(e => e.Text == "test comment"));

            await facade.DeleteComment(context.Comments.First(e => e.Text == "new test comment").Id);
            await facade.DeleteCaff(caff.CaffId);
        }


        [Fact]
        public async void test_get_image()
        {
            var caff = await facade.UploadCaff(request);
            var ciff = context.Images.First(e => e.CaffId == caff.CaffId);
            var img = await facade.GetImage(ciff.Id);
            Assert.Equal(ciff.Preview, img);
            await facade.DeleteCaff(caff.CaffId);
        }

        [Fact]
        public async void test_get_preview()
        {
            var caffId = await facade.UploadCaff(request);
            var preview = await facade.GetPreview(caffId.CaffId);
            var caff = context.Caffs.First(e => e.Id== caffId.CaffId);
            Assert.True(preview.Name == caff.CaffName);
            Assert.True(preview.Creator == caff.Creator);
            Assert.True(preview.CreatorDate == caff.CreatorDate);
            Assert.True(preview.CaffID == caff.Id);
            await facade.DeleteCaff(caffId.CaffId);
        }

        [Fact]
        public async void test_paged_search()
        {
            var caff = await facade.UploadCaff(request);
            var result = await facade.PagedSearch(new PagedSearchSpecification());

            var caffsFromContext = context.Caffs.Select(e => e).ToList();
            Assert.Equal(result.TotalCount, caffsFromContext.Count);
            
            await facade.DeleteCaff(caff.CaffId);
        }
    }
}