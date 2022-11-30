using CaffDal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CaffDal
{
    public partial class CaffDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public CaffDbContext(DbContextOptions options) : base(options) { }

        protected CaffDbContext() { }

        public DbSet<Caff> Caffs { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("Users");
            });
        }
    }
}
