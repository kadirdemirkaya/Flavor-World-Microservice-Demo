using BasketService.Domain.Aggregate;
using BasketService.Domain.Aggregate.Entities;
using Microsoft.EntityFrameworkCore;

namespace BasketService.Infrastructure.Persistence.Data
{
    public class BasketDbContext : DbContext
    {
        public BasketDbContext()
        {
        }
        public BasketDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Basket> Orders { get; private set; }
        public DbSet<BasketItem> BasketItems { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
