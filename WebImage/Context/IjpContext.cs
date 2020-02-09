using Microsoft.EntityFrameworkCore;
using WebImage.Context;

namespace WebImage
{
    public class IjpContext : DbContext
    {

        public IjpContext(DbContextOptions<IjpContext> options) : base(options)
        {
        }

        public DbSet<IjpFile> File { get; set; }
        public DbSet<IjpCategory> Category { get; set; }
        public DbSet<IjpGallery> Gallery {get;set;}
        public DbSet<IjpGalleryFile> GalleryFile { get; set; }



        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<IjpFile>().ToTable("FileContent");
            modelbuilder.Entity<IjpFile>().HasKey(x => x.FileId);

            modelbuilder.Entity<IjpCategory>().ToTable("Category");
            modelbuilder.Entity<IjpCategory>().HasKey(x => x.CategoryId);

            modelbuilder.Entity<IjpGallery>().ToTable("Gallery");
            modelbuilder.Entity<IjpGallery>().HasKey(x => x.GalleryId);

            modelbuilder.Entity<IjpGalleryFile>().ToTable("GalleryFile");
            modelbuilder.Entity<IjpGalleryFile>().HasKey(x => x.GalleryFileId);

            base.OnModelCreating(modelbuilder);
        }

    }
}