using BasketService.Application.IntegrationEvents.Events;
using BasketService.Domain.Models;
using BuildingBlock.Base.Abstractions;
using BuildingBlock.Redis;

namespace BasketService.Application.IntegrationEvents.EventHandlers
{
    public class OrderNotCompletedIntegrationEventHandler : IIntegrationEventHandler<OrderNotCompletedIntegrationEvent>
    {
        private readonly IRedisRepository<BasketModel> _redisBasketRepository;

        public OrderNotCompletedIntegrationEventHandler(IRedisRepository<BasketModel> redisBasketRepository)
        {
            _redisBasketRepository = redisBasketRepository;
        }

        public async Task Handle(OrderNotCompletedIntegrationEvent @event)
        {
            //_redisBasketRepository.Delete(BasketService.Domain.Constants.Constant.InMemory.InMemoryBasketItemKey, BasketService.Domain.Constants.Constant.InMemory.InMemoryBasketItemKeyId.ToString(), BuildingBlock.Base.Enums.RedisDataType.String);

            Serilog.Log.Error("Basket Created Process Not SUccesfully !");
        }
    }
}
