using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Enums;
using BuildingBlock.RabbitMQ;
using BuildingBlock.Redis;

namespace BuildingBlock.Factory.Factories
{
    public static class EventBusFactory
    {
        public static IEventBus Create(EventBusConfig config, IServiceProvider serviceProvider)
        {
            return config.EventBusType switch
            {
                EventBusType.Redis => new EventBusRedis(config, serviceProvider),
                EventBusType.RabbitMQ => new EventBusRabbitMQ(config, serviceProvider),
                _ => new EventBusRabbitMQ(config, serviceProvider)
            };
        }
    }
}
