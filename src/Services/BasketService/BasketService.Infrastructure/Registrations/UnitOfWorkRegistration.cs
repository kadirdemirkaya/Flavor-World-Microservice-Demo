using AuthenticationService.Domain.Constants;
using BasketService.Application.Abstractions;
using BasketService.Application.Configurations;
using BasketService.Domain.Aggregate;
using BasketService.Domain.Aggregate.ValueObjects;
using BasketService.Infrastructure.Persistence.Data;
using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Enums;
using BuildingBlock.Factory.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace BasketService.Infrastructure.Registrations
{
    public static class UnitOfWord
    {
        public static IServiceCollection IUnitOfWorkRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            var sp = GetServiceProvider(services);

            var context = sp.GetRequiredService<BasketDbContext>();

            var pubService = sp.GetRequiredService<IPubEventService>();

            Func<string, Task> pubEvent = pubService.PublishDomainEventAsync;

            services.AddScoped<IUnitOfWork>(sp =>
            {
                return DatabaseFactory.CreateUnitOfWork<Basket, BasketId>(GetConfigs.GetDatabaseConfig, context, pubService.PublishDomainEventAsync, Constant.App.ApplicationName);
            });

            services.AddScoped<IUnitOfWork>(sp =>
            {
                return DatabaseFactory.CreateUnitOfWork<Basket, BasketId>(GetConfigs.GetDatabaseConfig, context, pubService.PublishDomainEventAsync, Constant.App.ApplicationName);
            });

            services.AddScoped<IUnitOfWork>(sp =>
            {
                return DatabaseFactory.CreateUnitOfWork<Basket, BasketId>(GetConfigs.GetDatabaseConfig, context, pubService.PublishDomainEventAsync, Constant.App.ApplicationName);
            });

            return services;
        }
        private static ServiceProvider GetServiceProvider(IServiceCollection services) => services.BuildServiceProvider();
    }
}
