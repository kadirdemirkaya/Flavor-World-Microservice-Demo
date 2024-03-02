using BasketService.Domain.Constants;
using BasketService.Domain.Models;
using BuildingBlock.Base.Enums;
using BuildingBlock.Redis;
using MediatR;

namespace BasketService.Application.Features.Commands.Basket.RemoveBasketItemInCache
{
    public class RemoveBasketItemInCacheCommandHandler : IRequestHandler<RemoveBasketItemInCacheCommand, RemoveBasketItemInCacheCommandResponse>
    {
        private readonly IRedisRepository<BasketItemModel> _redisRepository;
        private List<bool> _dbResults;
        public RemoveBasketItemInCacheCommandHandler(IRedisRepository<BasketItemModel> redisRepository)
        {
            _redisRepository = redisRepository;
            _dbResults = new();
        }

        public async Task<RemoveBasketItemInCacheCommandResponse> Handle(RemoveBasketItemInCacheCommand request, CancellationToken cancellationToken)
        {
            List<BasketItemModel> basketItemModels = _redisRepository.GetAll(Constant.InMemory.InMemoryBasketItemKey, RedisDataType.String).ToList();

            basketItemModels.Remove(basketItemModels.Find(b => b.ProductId == request.productId));

            bool listResult = _redisRepository.Delete(Constant.InMemory.InMemoryBasketItemKey, Constant.InMemory.InMemoryBasketItemKeyId.ToString(), RedisDataType.String);

            if (listResult)
                foreach (var basketItemModel in basketItemModels)
                    _dbResults.Add(_redisRepository.Create(Constant.InMemory.InMemoryBasketItemKey, basketItemModel, RedisDataType.String));

            return new(_dbResults.All(value => true));
        }
    }
}
