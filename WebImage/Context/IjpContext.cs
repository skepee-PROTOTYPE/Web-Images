using Microsoft.EntityFrameworkCore;
using WebImage.Context;

namespace WebImage
{
    public class IjpContext: DbContext
    {

        public IjpContext(DbContextOptions<IjpContext> options) : base(options)
        {
        }

        public DbSet<FileContent> FileContent { get; set; }



        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
           // modelbuilder.con

            modelbuilder.Entity<FileContent>().ToTable("FileContent");
            modelbuilder.Entity<FileContent>().HasKey(x => x.FileId);


            base.OnModelCreating(modelbuilder);


        }


    }
}