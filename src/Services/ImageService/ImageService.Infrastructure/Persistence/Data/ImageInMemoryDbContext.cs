using BuildingBlock.Base.Abstractions;
using ImageService.Domain.Aggregate;
using ImageService.Domain.Aggregate.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageService.Infrastructure.Persistence.Data
{
    //public class ImageInMemoryDbContext : DbContext
    //{
    //    public ImageInMemoryDbContext()
    //    {

    //    }
    //    public ImageInMemoryDbContext(DbContextOptions options) : base(options)
    //    {
    //    }

    //    public DbSet<Image> Images { get; set; }
    //    public DbSet<ImageUser> ImageUsers { get; set; }

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        modelBuilder.Ignore<List<IDomainEvent>>();
    //        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    //        base.OnModelCreating(modelBuilder);
    //    }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        base.OnConfiguring(optionsBuilder);
    //    }

    //}
}
