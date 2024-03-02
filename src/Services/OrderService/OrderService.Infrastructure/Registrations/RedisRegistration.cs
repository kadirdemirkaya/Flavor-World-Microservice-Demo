using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Enums;
using BuildingBlock.Factory.Factories;
using BuildingBlock.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Configurations;
using OrderService.Application.IntegrationEvents.EventHandlers;
using OrderService.Application.IntegrationEvents.Events;
using OrderService.Domain.Aggregate.OrderAggregate;
using OrderService.Domain.Aggregate.OrderAggregate.ValueObjects;
using OrderService.Domain.Models;

namespace OrderService.Infrastructure.Registrations
{
    public static class Redis
    {
        public static IServiceCollection RedisRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEventBus>(sp =>
            {
                return EventBusFactory.Create(GetConfigs.GetRedisDefaultConfig, sp);
            });

            services.AddScoped<IRedisService<Order, OrderId>>(sp =>
            {
                return RedisFactory<Order, OrderId>.CreateForEntity(GetConfigs.GetRedisConfig, sp);
            });

            services.AddScoped<IRedisRepository<OrderModel>>(sp =>
            {
                return RedisRepositoryFactory<OrderModel>.CreateRepository(GetConfigs.GetInMemoryConfig, sp);
            });

            services.AddSingleton<IEventBus>(sp =>
            {
                return EventBusFactory.Create(GetConfigs.GetRedisDefaultConfig, sp);
            });

            services.AddTransient<BasketConfirmedIntegrationEventHandler>();
            services.AddTransient<OrderCompletedIntegrationEventHandler>();
            services.AddTransient<OrderNotCompletedIntegrationEventHandler>();


            return services;
        }

        public static WebApplication RedisRegistrationApp(this WebApplication app, IServiceProvider serviceProvider)
        {
            var eventBus = serviceProvider.GetRequiredService<IEventBus>();

            eventBus.Subscribe<BasketConfirmedIntegrationEvent, BasketConfirmedIntegrationEventHandler>();
            eventBus.Subscribe<OrderCompletedIntegrationEvent, OrderCompletedIntegrationEventHandler>();
            eventBus.Subscribe<OrderNotCompletedIntegrationEvent, OrderNotCompletedIntegrationEventHandler>();

            return app;
        }

    }
}
