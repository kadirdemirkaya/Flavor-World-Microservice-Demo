using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Enums;
using BuildingBlock.Factory.Factories;
using BuildingBlock.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Configurations;
using ProductService.Application.IntegrationEvents.EventHandlers;
using ProductService.Application.IntegrationEvents.Events;
using ProductService.Domain.Aggregate.ProductAggregate;
using ProductService.Domain.Aggregate.ProductAggregate.ValueObjects;
using ProductService.Domain.Models;

namespace ProductService.Infrastructure.Registrations
{
    public static class Redis
    {
        public static IServiceCollection RedisRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEventBus>(sp =>
            {
                return EventBusFactory.Create(GetConfigs.GetRedisEventBusConfig, sp);
            });

            services.AddScoped<IRedisService<Product, ProductId>>(sp =>
            {
                return RedisFactory<Product, ProductId>.CreateForEntity(GetConfigs.GetRedisConfig, sp);
            });

            services.AddScoped<IRedisRepository<ProductService.Domain.Models.ProductModel>>(sp =>
            {
                return RedisRepositoryFactory<ProductService.Domain.Models.ProductModel>.CreateRepository(GetConfigs.GetInMemoryConfig, sp);
            });

            services.AddScoped<IRedisRepository<AllProductsModel>>(sp =>
            {
                return RedisRepositoryFactory<AllProductsModel>.CreateRepository(GetConfigs.GetInMemoryConfig, sp);
            });

            services.AddTransient<OrderCreatedIntegrationEventHandler>();

            return services;
        }

        public static WebApplication RedisRegistrationApp(this WebApplication app, IServiceProvider serviceProvider)
        {
            var eventBus = serviceProvider.GetRequiredService<IEventBus>();

            eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();

            return app;
        }
    }
}
