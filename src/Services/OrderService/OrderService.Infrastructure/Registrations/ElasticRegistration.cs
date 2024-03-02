using BuildingBlock.Base.Abstractions;
using BuildingBlock.Elasticsearch;
using BuildingBlock.Factory.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Configurations;
using OrderService.Domain.Aggregate.OrderAggregate;
using OrderService.Domain.Aggregate.OrderAggregate.ValueObjects;

namespace OrderService.Infrastructure.Registrations
{
    public static class Elastic
    {
        public static IServiceCollection ElasticRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.ElasticsearchExtension(GetConfigs.GetSearchConfig);

            services.AddScoped<IElastic<Order, OrderId>>(sp =>
            {
                return ElasticFactory<Order, OrderId>.CreateForEntity(GetConfigs.GetSearchConfig, sp);
            });

            return services;
        }
    }
}
