using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProductService.Infrastructure.Persistence.Data;
using ProductService.Application.Configurations;

namespace ProductService.Infrastructure
{
    public class DesignTimeContext : IDesignTimeDbContextFactory<ProductDbContext>
    {
        public ProductDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<ProductDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseSqlServer(GetConfigs.GetDbConnectionString);
            return new(dbContextOptionsBuilder.Options);
        }
    }
}
