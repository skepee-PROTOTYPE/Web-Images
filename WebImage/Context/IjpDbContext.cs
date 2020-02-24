using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebImage.DBContext
{
    public class IjpDbContext : IdentityDbContext
    {
        public IjpDbContext(DbContextOptions<IjpDbContext> options)
            : base(options)
        {
        }
    }
}
