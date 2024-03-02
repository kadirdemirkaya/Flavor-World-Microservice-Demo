using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OrderService.Infrastructure.Persistence.Data;
using OrderService.Application.Configurations;

namespace OrderService.Infrastructure
{
    public class DesignTimeContext : IDesignTimeDbContextFactory<OrderDbContext>
    {
        public OrderDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<OrderDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseSqlServer(GetConfigs.GetDbConnectionString);
            return new(dbContextOptionsBuilder.Options);
        }
    }
}
