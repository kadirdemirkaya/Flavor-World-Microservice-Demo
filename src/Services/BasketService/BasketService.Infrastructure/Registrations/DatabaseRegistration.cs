using BasketService.Application.Configurations;
using BasketService.Domain.Aggregate;
using BasketService.Domain.Aggregate.Entities;
using BasketService.Domain.Aggregate.ValueObjects;
using BasketService.Infrastructure.Persistence.Data;
using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Enums;
using BuildingBlock.Factory.Factories;
using BuildingBlock.MsSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BasketService.Infrastructure.Registrations
{
    public static class Database
    {
        public static IServiceCollection DatabaseServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BasketDbContext>(options =>
            {
                options.UseSqlServer(GetConfigs.GetDbConnectionString,
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(AssemblyReference.Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: System.TimeSpan.FromSeconds(30), null);
                });
            });

            var optionsBuilder = new DbContextOptionsBuilder<BasketDbContext>().UseSqlServer(GetConfigs.GetDbConnectionString);
            using var dbContext = new BasketDbContext(optionsBuilder.Options);

            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();

            services.MsSQLExtension(GetConfigs.GetMsSqlConfig);

            var sp = GetServiceProvider(services);
            var context = sp.GetRequiredService<BasketDbContext>();

            services.AddScoped<IWriteRepository<Basket, BasketId>>(sp =>
            {
                return DatabaseFactory<Basket, BasketId>.CreateForWriteEntity(GetConfigs.GetDatabaseConfig, context, sp);
            });

            services.AddScoped<IReadRepository<Basket, BasketId>>(sp =>
            {
                return DatabaseFactory<Basket, BasketId>.CreateForReadEntity(GetConfigs.GetDatabaseConfig, context, sp);
            });

            services.AddScoped<IWriteRepository<BasketItem, BasketItemId>>(sp =>
            {
                return DatabaseFactory<BasketItem, BasketItemId>.CreateForWriteEntity(GetConfigs.GetDatabaseConfig, context, sp);
            });

            services.AddScoped<IReadRepository<BasketItem, BasketItemId>>(sp =>
            {
                return DatabaseFactory<BasketItem, BasketItemId>.CreateForReadEntity(GetConfigs.GetDatabaseConfig, context, sp);
            });

            return services;
        }

        private static ServiceProvider GetServiceProvider(IServiceCollection services) => services.BuildServiceProvider();
    }
}
