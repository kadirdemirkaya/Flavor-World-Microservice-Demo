using BasketService.Domain.Aggregate.ValueObjects;
using BasketService.Domain.Models;
using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Enums;
using BuildingBlock.Base.Models;
using BuildingBlock.Redis;
using MediatR;
using BasketService.Domain.Constants;
using BasketService.Application.IntegrationEvents.Events;
using BasketService.Application.Abstractions;

namespace BasketService.Application.Features.Commands.Basket.ConfirmBasketForOrder
{
    public class ConfirmBasketForOrderCommandHandler : IRequestHandler<ConfirmBasketForOrderCommand, ConfirmBasketForOrderCommandResponse>
    {
        private readonly IRedisRepository<BasketModel> _redisBasketRepository;
        private readonly IRedisRepository<BasketItemModel> _redisBasketItemRepository;
        private readonly ITokenService _tokenService;
        private readonly IEventBus _eventBus;
        private readonly IUserInfoService _userInfoService;
        public ConfirmBasketForOrderCommandHandler(IRedisRepository<BasketModel> redisBasketRepository, IRedisRepository<BasketItemModel> redisBasketItemRepository, ITokenService tokenService, IEventBus eventBus, IUserInfoService userInfoService)
        {
            _redisBasketRepository = redisBasketRepository;
            _redisBasketItemRepository = redisBasketItemRepository;
            _tokenService = tokenService;
            _eventBus = eventBus;
            _userInfoService = userInfoService;
        }

        public async Task<ConfirmBasketForOrderCommandResponse> Handle(ConfirmBasketForOrderCommand request, CancellationToken cancellationToken)
        {
            List<BasketItemModel>? basketItems = _redisBasketItemRepository.GetAll(Constant.InMemory.InMemoryBasketItemKey, BuildingBlock.Base.Enums.RedisDataType.String).ToList();

            List<ProductDetailModel> productDetailModels = new();

            foreach (var bItem in basketItems)
                productDetailModels.Add(new() { ProductCount = bItem.ProductCount, ProductId = bItem.ProductId });

            UserModel? userModel = await _userInfoService.GetUserModel(request.token);

            BasketModel basketModel = new() { UserId = userModel.Id, Id = basketItems.First().BasketId, BasketStatus = Domain.Aggregate.Enums.BasketStatus.Active };

            await _redisBasketRepository.CreateAsync(Constant.InMemory.InMemoryBasketKey, basketModel, RedisDataType.String);

            BasketConfirmedIntegrationEvent basketConfirmedIntegrationEvent = new(userModel.Id, basketModel.Id, basketItems.Select(bi => bi.OrderDescription).First(), productDetailModels, request.token);

            _eventBus.Publish(basketConfirmedIntegrationEvent);

            return new(true);
        }
    }
}
