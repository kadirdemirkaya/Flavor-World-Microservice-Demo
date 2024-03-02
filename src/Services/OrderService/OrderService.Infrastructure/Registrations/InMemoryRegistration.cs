using BuildingBlock.Base.Abstractions;
using BuildingBlock.Factory.Factories;
using BuildingBlock.InMemory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Configurations;
using OrderService.Domain.Aggregate.OrderAggregate;
using OrderService.Domain.Aggregate.OrderAggregate.ValueObjects;
using OrderService.Infrastructure.Persistence.Data;

namespace OrderService.Infrastructure.Registrations
{
    public static class InMemory
    {
        public static IServiceCollection InMemoryServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderDbContext>(options => options.UseInMemoryDatabase(configuration["DbConnection"]!));

            var sp = GetServiceProvider(services);
            var context = sp.GetRequiredService<OrderDbContext>();

            services.InMemoryDbContext<Order, OrderId>(configuration);

            services.AddScoped<IUnitOfWork>(sp =>
            {
                return DatabaseFactory.CreateUnitOfWork(GetConfigs.GetDatabaseConfig, context);
            });

            services.AddScoped<IInMemoryRepository<Order, OrderId>>(sp =>
            {
                return InMemoryFactory<Order, OrderId>.CreateForEntity(GetConfigs.GetInMemoryConfig, context, sp);
            });

            return services;
        }

        private static ServiceProvider GetServiceProvider(IServiceCollection services) => services.BuildServiceProvider();
    }
}
