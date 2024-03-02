using BasketService.Application.Configurations;
using BasketService.Domain.Aggregate;
using BasketService.Domain.Aggregate.ValueObjects;
using BasketService.Infrastructure.Persistence.Data;
using BuildingBlock.Base.Abstractions;
using BuildingBlock.Factory.Factories;
using BuildingBlock.InMemory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BasketService.Infrastructure.Registrations
{
    public static class InMemory
    {
        public static IServiceCollection InMemoryServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BasketDbContext>(options => options.UseInMemoryDatabase(GetConfigs.GetDbConnectionString));

            var sp = GetServiceProvider(services);
            var context = sp.GetRequiredService<BasketDbContext>();

            services.InMemoryDbContext<Basket, BasketId>(configuration);

            services.AddScoped<IUnitOfWork>(sp =>
            {
                return DatabaseFactory.CreateUnitOfWork(GetConfigs.GetDatabaseConfig, context);
            });

            services.AddScoped<IInMemoryRepository<Basket, BasketId>>(sp =>
            {
                return InMemoryFactory<Basket, BasketId>.CreateForEntity(GetConfigs.GetInMemoryConfig, context, sp);
            });

            return services;
        }
        private static ServiceProvider GetServiceProvider(IServiceCollection services) => services.BuildServiceProvider();
    }
}
