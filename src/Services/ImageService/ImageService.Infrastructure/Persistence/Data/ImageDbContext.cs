using BuildingBlock.Base.Abstractions;
using ImageService.Domain.Aggregate;
using ImageService.Domain.Aggregate.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageService.Infrastructure.Persistence.Data
{
    public class ImageDbContext : DbContext
    {
        public ImageDbContext()
        {

        }
        public ImageDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Image> Images { get; set; }
        public DbSet<ImageUser> ImageUsers { get; set; }
        public DbSet<ImageProduct> ImageProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<List<IDomainEvent>>();
            modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
