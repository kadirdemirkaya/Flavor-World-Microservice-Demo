using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Aggregate.ProductAggregate;

namespace ProductService.Infrastructure.Persistence.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext()
        {
        }
        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Product> Products { get; private set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
