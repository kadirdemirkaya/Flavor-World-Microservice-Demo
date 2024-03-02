using BasketService.Application.Configurations;
using BasketService.Application.IntegrationEvents.EventHandlers;
using BasketService.Application.IntegrationEvents.Events;
using BasketService.Domain.Aggregate;
using BasketService.Domain.Aggregate.ValueObjects;
using BasketService.Domain.Models;
using BuildingBlock.Base.Abstractions;
using BuildingBlock.Factory.Factories;
using BuildingBlock.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BasketService.Infrastructure.Registrations
{
    public static class Redis
    {
        public static IServiceCollection RedisRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEventBus>(sp =>
            {
                return EventBusFactory.Create(GetConfigs.GetRedisEventBusConfig, sp);
            });

            services.AddScoped<IRedisService<Basket, BasketId>>(sp =>
            {
                return RedisFactory<Basket, BasketId>.CreateForEntity(GetConfigs.GetRedisConfig, sp);
            });

            services.AddScoped<IRedisRepository<BasketModel>>(sp =>
            {
                return RedisRepositoryFactory<BasketModel>.CreateRepository(GetConfigs.GetInMemoryConfig, sp);
            });

            services.AddScoped<IRedisRepository<BasketItemModel>>(sp =>
            {
                return RedisRepositoryFactory<BasketItemModel>.CreateRepository(GetConfigs.GetInMemoryConfig, sp);
            });

            services.AddSingleton<IEventBus>(sp =>
            {
                return EventBusFactory.Create(GetConfigs.GetRedisDefaultConfig, sp);
            });

            services.AddTransient<OrderCompletedIntegrationEventHandler>();
            services.AddTransient<OrderNotCompletedIntegrationEventHandler>();
            services.AddTransient<OrderNotCreatedIntegrationEventHandler>();

            return services;
        }

        public static WebApplication RedisRegistrationApp(this WebApplication app, IServiceProvider serviceProvider)
        {
            var eventBus = serviceProvider.GetRequiredService<IEventBus>();

            eventBus.Subscribe<OrderCompletedIntegrationEvent, OrderCompletedIntegrationEventHandler>();
            eventBus.Subscribe<OrderNotCompletedIntegrationEvent, OrderNotCompletedIntegrationEventHandler>();
            eventBus.Subscribe<OrderNotCreatedIntegrationEvent, OrderNotCreatedIntegrationEventHandler>();

            return app;
        }

        private static string GetRedisUrl(IConfiguration configuration) => configuration["RedisConfig:RedisConnection"];
    }
}
