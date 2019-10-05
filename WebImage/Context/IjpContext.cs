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



    }
}