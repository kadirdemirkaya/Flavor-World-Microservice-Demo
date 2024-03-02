using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Enums;
using BuildingBlock.Factory.Factories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Configurations;
using OrderService.Application.IntegrationEvents.EventHandlers;
using OrderService.Application.IntegrationEvents.Events;
using RabbitMQ.Client;

namespace OrderService.Infrastructure.Registrations
{
    public static class RabbitMq
    {
        public static IServiceCollection RabbitMqRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEventBus>(sp =>
            {
                return EventBusFactory.Create(GetConfigs.GetRabbitMqDefaultEventBusConfig, sp);
            });

            services.AddTransient<BasketConfirmedIntegrationEventHandler>();
            services.AddTransient<OrderCompletedIntegrationEventHandler>();
            services.AddTransient<OrderNotCompletedIntegrationEventHandler>();

            return services;
        }

        public static WebApplication RabbitMqRegistrationApp(this WebApplication app, IServiceProvider serviceProvider)
        {
            var eventBus = serviceProvider.GetRequiredService<IEventBus>();

            eventBus.Subscribe<BasketConfirmedIntegrationEvent, BasketConfirmedIntegrationEventHandler>();
            eventBus.Subscribe<OrderCompletedIntegrationEvent, OrderCompletedIntegrationEventHandler>();
            eventBus.Subscribe<OrderNotCompletedIntegrationEvent, OrderNotCompletedIntegrationEventHandler>();

            return app;
        }

        private static EventBusConfig GetRabbitMQManuelConfig(IConfiguration configuration)
        {
            return new EventBusConfig()
            {
                ConnectionRetryCount = int.Parse(configuration["RabbitMq:ConnectionRetryCount"]),
                SubscriberClientAppName = configuration["RabbitMq:SubscriberClientAppName"],
                DefaultTopicName = configuration["RabbitMq:DefaultTopicName"],
                EventBusType = EventBusType.RabbitMQ,
                EventNameSuffix = configuration["RabbitMq:EventNameSuffix"],
                Connection = new ConnectionFactory()
                {
                    HostName = configuration["RabbitMq:Host"],
                    Port = int.Parse(configuration["RabbitMq:Port"]),
                    UserName = configuration["RabbitMq:UserName"],
                    Password = configuration["RabbitMq:Password"],
                    VirtualHost = "/"
                }
            };
        }
    }
}
