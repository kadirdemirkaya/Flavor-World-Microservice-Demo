using BasketService.Application.Configurations;
using BasketService.Application.IntegrationEvents.EventHandlers;
using BasketService.Application.IntegrationEvents.Events;
using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Enums;
using BuildingBlock.Factory.Factories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace BasketService.Infrastructure.Registrations
{
    public static class RabbitMq
    {
        public static IServiceCollection RabbitMqRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEventBus>(sp =>
            {
                return EventBusFactory.Create(GetConfigs.GetRabbitMqDefaultEventBusConfig, sp);
            });

            services.AddTransient<OrderCompletedIntegrationEventHandler>();
            services.AddTransient<OrderNotCompletedIntegrationEventHandler>();
            services.AddTransient<OrderNotCreatedIntegrationEventHandler>();

            return services;
        }

        public static WebApplication RabbitMqRegistrationApp(this WebApplication app, IServiceProvider serviceProvider)
        {
            var eventBus = serviceProvider.GetRequiredService<IEventBus>();

            eventBus.Subscribe<OrderCompletedIntegrationEvent, OrderCompletedIntegrationEventHandler>();
            eventBus.Subscribe<OrderNotCompletedIntegrationEvent, OrderNotCompletedIntegrationEventHandler>();
            eventBus.Subscribe<OrderNotCreatedIntegrationEvent, OrderNotCreatedIntegrationEventHandler>();

            return app;
        }

        private static EventBusConfig GetRabbitMQManuelConfig(IConfiguration configuration)
            => new EventBusConfig()
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
