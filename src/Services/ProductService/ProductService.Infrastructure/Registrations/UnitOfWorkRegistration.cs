using BuildingBlock.Base.Abstractions;
using BuildingBlock.Factory.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Abstractions;
using ProductService.Application.Configurations;
using ProductService.Domain.Aggregate.ProductAggregate;
using ProductService.Domain.Aggregate.ProductAggregate.ValueObjects;
using ProductService.Infrastructure.Persistence.Data;

namespace ProductService.Infrastructure.Registrations
{
    public static class UnitOfWord
    {
        public static IServiceCollection UnitOfWorkRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            var sp = GetServiceProvider(services);

            var context = sp.GetRequiredService<ProductDbContext>();

            var pubService = sp.GetRequiredService<IPubEventService>();

            Func<string, Task> pubEvent = pubService.PublishDomainEventAsync;

            services.AddScoped<IUnitOfWork>(sp =>
            {
                return DatabaseFactory.CreateUnitOfWork<Product, ProductId>(GetConfigs.GetDatabaseConfig, context, pubService.PublishDomainEventAsync, ProductService.Domain.Constants.Constant.App.ApplicationName);
            });

            return services;
        }

        private static ServiceProvider GetServiceProvider(IServiceCollection services) => services.BuildServiceProvider();
    }
}
