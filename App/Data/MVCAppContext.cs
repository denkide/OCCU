using Microsoft.EntityFrameworkCore;
using MVCApp.Models;

namespace MVCApp.Data
{
    public class MVCAppContext : DbContext
    {
        public MVCAppContext(DbContextOptions<MVCAppContext> options)
            : base(options)
        {
        }

        public DbSet<ProductStatus> Status { get; set; }

        public DbSet<MVCApp.Models.Product> Product { get; set; }

    }
}
