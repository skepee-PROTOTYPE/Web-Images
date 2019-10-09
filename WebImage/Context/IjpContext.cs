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



        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
           // modelbuilder.con

            modelbuilder.Entity<IjpFile>().ToTable("FileContent");
            modelbuilder.Entity<IjpFile>().HasKey(x => x.FileId);


            base.OnModelCreating(modelbuilder);


        }


    }
}