using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Enums;
using BuildingBlock.Factory.Factories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Configurations;
using ProductService.Application.IntegrationEvents.EventHandlers;
using ProductService.Application.IntegrationEvents.Events;
using RabbitMQ.Client;

namespace ProductService.Infrastructure.Registrations
{
    public static class RabbitMq
    {
        public static IServiceCollection RabbitMqRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEventBus>(sp =>
            {
                return EventBusFactory.Create(GetConfigs.GetRabbitMqDefaultEventBusConfig, sp);
            });

            services.AddTransient<OrderCreatedIntegrationEventHandler>();

            return services;
        }

        public static WebApplication RabbitMqRegistrationApp(this WebApplication app, IServiceProvider serviceProvider)
        {
            var eventBus = serviceProvider.GetRequiredService<IEventBus>();

            eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();

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

        private static EventBusConfig GetRabbitMQDefaultConfig(IConfiguration configuration)
        {
            return new EventBusConfig
            {
                ConnectionRetryCount = int.Parse(configuration["RabbitMq:ConnectionRetryCount"]),
                EventNameSuffix = configuration["RabbitMq:EventNameSuffix"],
                SubscriberClientAppName = configuration["RabbitMq:SubscriberClientAppName"],
                EventBusType = EventBusType.RabbitMQ
            };
        }
    }
}
