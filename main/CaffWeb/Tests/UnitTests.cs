using CaffDal;
using Microsoft.EntityFrameworkCore;
using CaffDal.Entities;

namespace Tests
{
    public class UnitTests
    {
        CaffDbContext context;
        public UnitTests() 
        {
            context = buildContext();
        }

        private CaffDbContext buildContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CaffDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            return new CaffDbContext(optionsBuilder.Options);
        }

        [Fact]
        public void test_add_caff_to_db()
        {
            var caff = new Caff("creator", new byte[] { });
            caff.CreatorDate = new DateTime();
            caff.NumberOfFrames = 0;
            caff.UserId = 0;
            caff.CaffName = "caff";
            context.Caffs.Add(caff);
            context.SaveChanges();
            Assert.Single(context.Caffs);
            context.Caffs.Remove(caff);
            context.SaveChanges();
        }

        [Fact]
        public void test_delete_caff_from_db()
        {
            var caff = new Caff("creator", new byte[] { });
            caff.CreatorDate = new DateTime();
            caff.NumberOfFrames = 0;
            caff.UserId = 0;
            caff.CaffName = "caff";
            context.Caffs.Add(caff);
            context.SaveChanges();
            Assert.Single(context.Caffs);
            context.Caffs.Remove(caff);
            context.SaveChanges();
            Assert.Empty(context.Caffs);
        }

        [Fact]
        public void test_get_caffs_from_db()
        {
            var caff = new Caff("creator", new byte[] { });
            caff.CreatorDate = new DateTime();
            caff.NumberOfFrames = 0;
            caff.UserId = 0;
            caff.CaffName = "caff";
            context.Caffs.Add(caff);
            context.SaveChanges();
            var caffResult = context.Caffs.First(e => e.CaffName == "caff");
            Assert.Equal("creator", caffResult.Creator);
            Assert.Equal(0, caffResult.NumberOfFrames);
            Assert.Equal(0, caffResult.UserId);
            Assert.Empty(caffResult.RawCaff);
            context.Caffs.Remove(caff);
            context.SaveChanges();
        }

        [Fact]
        public void test_add_comment_to_db()
        {
            var comment = new Comment("comment");
            context.Comments.Add(comment);
            context.SaveChanges();
            Assert.Single(context.Comments);
            context.Comments.Remove(comment);
            context.SaveChanges();
        }

        [Fact]
        public void test_delete_comment_from_db()
        {
            var comment = new Comment("comment");
            context.Comments.Add(comment);
            context.SaveChanges();
            Assert.Single(context.Comments);
            context.Comments.Remove(comment);
            context.SaveChanges();
            Assert.Empty(context.Comments);
        }

        [Fact]
        public void test_get_comment_from_db()
        {
            var comment = new Comment("comment");
            comment.CaffId = 1;
            context.Comments.Add(comment);
            context.SaveChanges();
            var result = context.Comments.First(e => e.Text== comment.Text);
            Assert.Equal(1, result.CaffId);
            context.Comments.Remove(comment);
            context.SaveChanges();
        }
    }
}
