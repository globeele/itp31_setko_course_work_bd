using CourseWorkDb.Models.Tables;
using Microsoft.EntityFrameworkCore;

namespace CourseWorkDb.Models
{
    public partial class RationingDbContext : DbContext
    {
        public RationingDbContext()
        {
        }

        public RationingDbContext(DbContextOptions<RationingDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Output> Outputs { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Release> Releases { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }
    }
}
