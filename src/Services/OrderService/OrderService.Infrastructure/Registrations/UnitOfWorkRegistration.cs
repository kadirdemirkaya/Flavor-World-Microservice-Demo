using BuildingBlock.Base.Abstractions;
using BuildingBlock.Factory.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Abstractions;
using OrderService.Application.Configurations;
using OrderService.Domain.Aggregate.OrderAggregate;
using OrderService.Domain.Aggregate.OrderAggregate.ValueObjects;
using OrderService.Infrastructure.Persistence.Data;

namespace OrderService.Infrastructure.Registrations
{
    public static class UnitOfWord
    {
        public static IServiceCollection UnitOfWorkRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            var sp = GetServiceProvider(services);

            var context = sp.GetRequiredService<OrderDbContext>();

            var pubService = sp.GetRequiredService<IPubEventService>();

            Func<string, Task> pubEvent = pubService.PublishDomainEventAsync;

            services.AddScoped<IUnitOfWork>(sp =>
            {
                return DatabaseFactory.CreateUnitOfWork<Order, OrderId>(GetConfigs.GetDatabaseConfig, context, pubService.PublishDomainEventAsync, OrderService.Domain.Constants.Constant.App.ApplicationName);
            });

            return services;
        }
        private static ServiceProvider GetServiceProvider(IServiceCollection services) => services.BuildServiceProvider();
    }
}
