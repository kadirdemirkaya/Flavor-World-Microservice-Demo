using BuildingBlock.Base.Abstractions;
using BuildingBlock.Factory.Factories;
using BuildingBlock.MsSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Configurations;
using OrderService.Domain.Aggregate.OrderAggregate;
using OrderService.Domain.Aggregate.OrderAggregate.ValueObjects;
using OrderService.Infrastructure.Persistence.Data;

namespace OrderService.Infrastructure.Registrations
{
    public static class Database
    {
        public static IServiceCollection DatabaseServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderDbContext>(options =>
            {
                options.UseSqlServer(GetConfigs.GetDbConnectionString,
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(AssemblyReference.Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: System.TimeSpan.FromSeconds(30), null);
                });
            });

            var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>().UseSqlServer(GetConfigs.GetDbConnectionString);
            using var dbContext = new OrderDbContext(optionsBuilder.Options);

            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();

            services.MsSQLExtension(GetConfigs.GetMsSqlConfig);

            var sp = GetServiceProvider(services);
            var context = sp.GetRequiredService<OrderDbContext>();

            services.AddScoped<IWriteRepository<Order, OrderId>>(sp =>
            {
                return DatabaseFactory<Order, OrderId>.CreateForWriteEntity(GetConfigs.GetDatabaseConfig, context, sp);
            });

            services.AddScoped<IReadRepository<Order, OrderId>>(sp =>
            {
                return DatabaseFactory<Order, OrderId>.CreateForReadEntity(GetConfigs.GetDatabaseConfig, context, sp);
            });
            return services;
        }

        private static ServiceProvider GetServiceProvider(IServiceCollection services) => services.BuildServiceProvider();
    }
}
