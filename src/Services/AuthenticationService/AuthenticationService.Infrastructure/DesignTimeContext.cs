using AuthenticationService.Application.Configurations;
using AuthenticationService.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AuthenticationService.Infrastructure
{
    public class DesignTimeContext : IDesignTimeDbContextFactory<AuthenticationDbContext>
    {
        public AuthenticationDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<AuthenticationDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseSqlServer(GetConfigs.GetDbConnectionString);
            return new(dbContextOptionsBuilder.Options);
        }
    }
}
