using Microsoft.EntityFrameworkCore;
using WebImage.Context;

namespace WebImage
{
    public class IjpContext: DbContext
    {

        public IjpContext(DbContextOptions<IjpContext> options) : base(options)
        {
        }

        public DbSet<IjpFile> File { get; set; }
        public DbSet<IjpCategory> Category { get; set; }
        public DbSet<IjpUser> User { get; set; }


        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<IjpFile>().ToTable("FileContent");
            modelbuilder.Entity<IjpFile>().HasKey(x => x.FileId);

            modelbuilder.Entity<IjpCategory>().ToTable("Category");
            modelbuilder.Entity<IjpCategory>().HasKey(x => x.CategoryId);

            modelbuilder.Entity<IjpUser>().ToTable("User");
            modelbuilder.Entity<IjpUser>().HasKey(x => x.UserId);

            base.OnModelCreating(modelbuilder);
        }

    }
}