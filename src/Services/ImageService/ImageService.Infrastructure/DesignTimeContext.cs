using ImageService.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ImageService.Application.Configurations;

namespace ImageService.Infrastructure
{
    public class DesignTimeContext : IDesignTimeDbContextFactory<ImageDbContext>
    {
        public ImageDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<ImageDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseSqlServer(GetConfigs.GetDbConnectionString);
            return new(dbContextOptionsBuilder.Options);
        }
    }
}
