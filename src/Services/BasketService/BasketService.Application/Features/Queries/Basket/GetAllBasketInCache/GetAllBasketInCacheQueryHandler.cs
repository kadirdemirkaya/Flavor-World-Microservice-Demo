using AutoMapper;
using BasketService.Domain.Aggregate.Entities;
using BasketService.Domain.Constants;
using BasketService.Domain.Models;
using BuildingBlock.Redis;
using MediatR;

namespace BasketService.Application.Features.Queries.Basket.GetAllBasketInCache
{
    public class GetAllBasketInCacheQueryHandler : IRequestHandler<GetAllBasketInCacheQuery, GetAllBasketInCacheQueryResponse>
    {
        private readonly IRedisRepository<BasketItemModel> _redisRepository;
        private List<BasketItem> basketItems;
        private readonly IMapper _mapper;

        public GetAllBasketInCacheQueryHandler(IRedisRepository<BasketItemModel> redisRepository, IMapper mapper)
        {
            _redisRepository = redisRepository;
            basketItems = new();
            _mapper = mapper;
        }

        public async Task<GetAllBasketInCacheQueryResponse> Handle(GetAllBasketInCacheQuery request, CancellationToken cancellationToken)
        {
            var cacheList = _redisRepository.GetAll(Constant.InMemory.InMemoryBasketItemKey, BuildingBlock.Base.Enums.RedisDataType.String).ToList();
            return new(_mapper.Map<List<BasketItem>>(cacheList));
        }
    }
}
