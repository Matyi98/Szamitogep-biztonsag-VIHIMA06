using CaffDal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaffDal
{
    public partial class CaffDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public CaffDbContext(DbContextOptions options) : base(options) { }

        protected CaffDbContext() { }

        public virtual DbSet<Caff> Caffs { get; set; }
        public virtual DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(e => {
                e.ToTable("Users");
            });
        }
    }
}
