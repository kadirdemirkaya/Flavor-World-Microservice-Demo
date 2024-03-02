using BasketService.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using BasketService.Application.Configurations;

namespace BasketService.Infrastructure
{
    public class DesignTimeContext : IDesignTimeDbContextFactory<BasketDbContext>
    {
        public BasketDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<BasketDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseSqlServer(GetConfigs.GetDbConnectionString);
            return new(dbContextOptionsBuilder.Options);
        }
    }
}
