using BasketService.Domain.Constants;
using BasketService.Domain.Models;
using BuildingBlock.Base.Enums;
using BuildingBlock.Redis;
using MediatR;

namespace BasketService.Application.Features.Queries.Basket.GetBasketItem
{
    public class GetBasketItemQueryHandler : IRequestHandler<GetBasketItemQuery, GetBasketItemQueryResponse>
    {
        private readonly IRedisRepository<BasketItemModel> _redisRepository;

        public GetBasketItemQueryHandler(IRedisRepository<BasketItemModel> redisRepository)
        {
            _redisRepository = redisRepository;
        }

        public async Task<GetBasketItemQueryResponse> Handle(GetBasketItemQuery request, CancellationToken cancellationToken)
        {
            var basketItemModel = _redisRepository.GetById(Constant.InMemory.InMemoryBasketKey, Constant.InMemory.InMemoryBasketItemKeyId.ToString(), RedisDataType.String);
            return new(new() { BasketId = basketItemModel.BasketId, ProductId = basketItemModel.ProductId });
        }
    }
}
