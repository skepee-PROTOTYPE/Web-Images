using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebImage
{
    public class IjpDbContext : IdentityDbContext
    {
        public IjpDbContext(DbContextOptions<IjpDbContext> options)
            : base(options)
        {
        }
    }
}
