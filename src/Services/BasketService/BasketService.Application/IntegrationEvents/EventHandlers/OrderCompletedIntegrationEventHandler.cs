using BasketService.Application.Features.Commands.Basket.ConfirmBasketInCache;
using BasketService.Application.IntegrationEvents.Events;
using BasketService.Domain.Models;
using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Enums;
using BuildingBlock.Redis;
using MediatR;

namespace BasketService.Application.IntegrationEvents.EventHandlers
{
    public class OrderCompletedIntegrationEventHandler : IIntegrationEventHandler<OrderCompletedIntegrationEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisRepository<BasketModel> _redisBasketRepository;
        private readonly IRedisRepository<BasketItemModel> _redisBasketItemRepository;
        private readonly IMediator _mediator;
        private readonly ITokenService _tokenService;

        public OrderCompletedIntegrationEventHandler(IUnitOfWork unitOfWork, IRedisRepository<BasketModel> redisBasketRepository, IRedisRepository<BasketItemModel> redisBasketItemRepository, IMediator mediator, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _redisBasketRepository = redisBasketRepository;
            _redisBasketItemRepository = redisBasketItemRepository;
            _mediator = mediator;
            _tokenService = tokenService;
        }

        public async Task Handle(OrderCompletedIntegrationEvent @event)
        {
            var basketModel = _redisBasketRepository.GetById(BasketService.Domain.Constants.Constant.InMemory.InMemoryBasketKey, BasketService.Domain.Constants.Constant.InMemory.InMemoryBasketKeyId.ToString(), RedisDataType.String);

            var basketItemModel = _redisBasketItemRepository.GetAll(BasketService.Domain.Constants.Constant.InMemory.InMemoryBasketItemKey, RedisDataType.String).ToList();

            ConfirmBasketInCacheCommand confirmBasketInCache = new(basketItemModel, @event.Token);

            ConfirmBasketInCacheCommandResponse response = await _mediator.Send(confirmBasketInCache);

            if (response.result)
            {
                _redisBasketRepository.Delete(BasketService.Domain.Constants.Constant.InMemory.InMemoryBasketKey, BasketService.Domain.Constants.Constant.InMemory.InMemoryBasketKeyId.ToString(), RedisDataType.String);
                _redisBasketItemRepository.Delete(BasketService.Domain.Constants.Constant.InMemory.InMemoryBasketItemKey, BasketService.Domain.Constants.Constant.InMemory.InMemoryBasketItemKeyId.ToString(), RedisDataType.String);
                Serilog.Log.Information("Basket Confirmed");
            }
            Serilog.Log.Error("Basket Not Confirmed");
        }
    }
}
