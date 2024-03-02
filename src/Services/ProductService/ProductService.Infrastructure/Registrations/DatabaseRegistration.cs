using BuildingBlock.Base.Abstractions;
using BuildingBlock.Factory.Factories;
using BuildingBlock.MsSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Configurations;
using ProductService.Domain.Aggregate.ProductAggregate;
using ProductService.Domain.Aggregate.ProductAggregate.ValueObjects;
using ProductService.Infrastructure.Persistence.Data;

namespace ProductService.Infrastructure.Registrations
{
    public static class Database
    {
        public static IServiceCollection DatabaseServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProductDbContext>(options =>
            {
                options.UseSqlServer(GetConfigs.GetDbConnectionString,
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(AssemblyReference.Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: System.TimeSpan.FromSeconds(30), null);
                });
            });

            var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>().UseSqlServer(GetConfigs.GetDbConnectionString);
            using var dbContext = new ProductDbContext(optionsBuilder.Options);

            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();

            services.MsSQLExtension(GetConfigs.GetMsSqlConfig);

            var sp = GetServiceProvider(services);
            var context = sp.GetRequiredService<ProductDbContext>();

            services.AddScoped<IWriteRepository<Product, ProductId>>(sp =>
            {
                return DatabaseFactory<Product, ProductId>.CreateForWriteEntity(GetConfigs.GetDatabaseConfig, context, sp);
            });

            services.AddScoped<IReadRepository<Product, ProductId>>(sp =>
            {
                return DatabaseFactory<Product, ProductId>.CreateForReadEntity(GetConfigs.GetDatabaseConfig, context, sp);
            });

            return services;
        }

        private static ServiceProvider GetServiceProvider(IServiceCollection services) => services.BuildServiceProvider();
    }
}
