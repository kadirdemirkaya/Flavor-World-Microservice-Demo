using BasketService.Domain.Constants;
using BasketService.Domain.Models;
using BuildingBlock.Redis;
using MediatR;

namespace BasketService.Application.Features.Commands.Basket.AddBasketInCache
{
    public class AddBasketInCacheCommandHandler : IRequestHandler<AddBasketInCacheCommand, AddBasketInCacheCommandResponse>
    {
        private readonly IRedisRepository<BasketItemModel> _redisRepository;
        private List<bool> _dbResults;
        bool _deleteBool;
        bool anyMatch;

        public AddBasketInCacheCommandHandler(IRedisRepository<BasketItemModel> redisRepository)
        {
            _redisRepository = redisRepository;
            _dbResults = new();
            _deleteBool = false;
            anyMatch = false;
        }

        public async Task<AddBasketInCacheCommandResponse> Handle(AddBasketInCacheCommand request, CancellationToken cancellationToken)
        {
            List<BasketItemModel>? basketItems = _redisRepository.GetAll(Constant.InMemory.InMemoryBasketItemKey, BuildingBlock.Base.Enums.RedisDataType.String).ToList();

            if (basketItems is not null && basketItems.Count() > 0)
                anyMatch = !request.addBasketDto.BasketItems.Any(item2 => item2.BasketId == basketItems.First().BasketId);
            else
                anyMatch = false;

            if (anyMatch)
            {
                _deleteBool = _redisRepository.Delete(Constant.InMemory.InMemoryBasketItemKey, Constant.InMemory.InMemoryBasketItemKeyId.ToString(), BuildingBlock.Base.Enums.RedisDataType.String);
            }

            if (basketItems is not null && basketItems.Count() > 0 && !anyMatch)
            {
                foreach (var basketItem in request.addBasketDto.BasketItems)
                    basketItems.Add(new() { Id = Guid.NewGuid(), BasketId = basketItem.BasketId, ProductId = basketItem.ProductId, ProductCount = basketItem.ProductCount });

                _deleteBool = _redisRepository.Delete(Constant.InMemory.InMemoryBasketItemKey, Constant.InMemory.InMemoryBasketItemKeyId.ToString(), BuildingBlock.Base.Enums.RedisDataType.String);

                if (_deleteBool)
                {
                    foreach (var basketItem in basketItems)
                    {
                        _dbResults.Add(_redisRepository.Create(Constant.InMemory.InMemoryBasketItemKey, new() { Id = Constant.InMemory.InMemoryBasketItemKeyId, BasketId = basketItem.BasketId, ProductId = basketItem.ProductId, OrderDescription = request.addBasketDto.orderDescription, ProductCount = basketItem.ProductCount }, BuildingBlock.Base.Enums.RedisDataType.String));
                    }
                    return new(_dbResults.All(value => true));
                }
                return new(false);
            }
            else
            {
                foreach (var basketItem in request.addBasketDto.BasketItems)
                    _dbResults.Add(_redisRepository.Create(Constant.InMemory.InMemoryBasketItemKey, new() { Id = Constant.InMemory.InMemoryBasketItemKeyId, BasketId = basketItem.BasketId, ProductId = basketItem.ProductId, OrderDescription = request.addBasketDto.orderDescription, ProductCount = basketItem.ProductCount }, BuildingBlock.Base.Enums.RedisDataType.String));
                if (_dbResults != null && _dbResults.All(value => value))
                    return new(true);
                else
                    return new(false);
            }
        }
    }
}
