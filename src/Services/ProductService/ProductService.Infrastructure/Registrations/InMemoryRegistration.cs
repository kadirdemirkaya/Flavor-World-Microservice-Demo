using BuildingBlock.Base.Abstractions;
using BuildingBlock.Factory.Factories;
using BuildingBlock.InMemory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Configurations;
using ProductService.Domain.Aggregate.ProductAggregate;
using ProductService.Domain.Aggregate.ProductAggregate.ValueObjects;
using ProductService.Infrastructure.Persistence.Data;

namespace ProductService.Infrastructure.Registrations
{
    public static class InMemory
    {
        public static IServiceCollection InMemoryServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProductDbContext>(options => options.UseInMemoryDatabase(GetConfigs.GetDbConnectionString));

            var sp = GetServiceProvider(services);
            var context = sp.GetRequiredService<ProductDbContext>();

            services.InMemoryDbContext<Product, ProductId>(configuration);

            services.AddScoped<IUnitOfWork>(sp =>
            {
                return DatabaseFactory.CreateUnitOfWork(GetConfigs.GetDatabaseConfig, context);
            });

            services.AddScoped<IInMemoryRepository<Product, ProductId>>(sp =>
            {
                return InMemoryFactory<Product, ProductId>.CreateForEntity(GetConfigs.GetInMemoryConfig, context, sp);
            });

            return services;
        }

        private static ServiceProvider GetServiceProvider(IServiceCollection services) => services.BuildServiceProvider();
    }
}
