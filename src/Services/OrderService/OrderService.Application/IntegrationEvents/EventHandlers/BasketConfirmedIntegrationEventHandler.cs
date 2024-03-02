using BuildingBlock.Base.Abstractions;
using OrderService.Application.IntegrationEvents.Events;
using OrderService.Domain.Models;
using BuildingBlock.Redis;

namespace OrderService.Application.IntegrationEvents.EventHandlers
{
    public class BasketConfirmedIntegrationEventHandler : IIntegrationEventHandler<BasketConfirmedIntegrationEvent>
    {
        private readonly IRedisRepository<OrderModel> _redisOrderRepository;
        private readonly IEventBus _eventBus;

        public BasketConfirmedIntegrationEventHandler(IRedisRepository<OrderModel> redisOrderRepository, IEventBus eventBus)
        {
            _redisOrderRepository = redisOrderRepository;
            _eventBus = eventBus;
        }

        public async Task Handle(BasketConfirmedIntegrationEvent @event)
        {
            Guid orderId = Guid.NewGuid();

            if (@event.Token == null || @event.BasketId == null)
            {
                OrderNotCreatedIntegrationEvent orderNotCreated = new(@event.Token);
                _eventBus.Publish(orderNotCreated);
            }

            bool redisResult = await _redisOrderRepository.CreateAsync(OrderService.Domain.Constants.Constant.InMemory.InMemoryOrderKey, new() { BasketId = @event.BasketId, Description = @event.OrderDescription, OrderId = orderId, Id = Guid.NewGuid() }, BuildingBlock.Base.Enums.RedisDataType.String);

            if (redisResult)
            {
                OrderCreatedIntegrationEvent basketConfirmedIntegrationEvent = new OrderCreatedIntegrationEvent(@event.UserId, @event.BasketId, orderId, @event.OrderDescription, @event.ProductDetailModels, @event.Token);

                _eventBus.Publish(basketConfirmedIntegrationEvent);
            }
        }
    }
}
