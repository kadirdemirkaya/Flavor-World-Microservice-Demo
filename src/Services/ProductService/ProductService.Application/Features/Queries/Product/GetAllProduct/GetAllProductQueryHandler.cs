using AutoMapper;
using BuildingBlock.Redis;
using MediatR;
using ProductService.Domain.Models;
using ProductService.Domain.Constants;
using BuildingBlock.Base.Enums;

namespace ProductService.Application.Features.Queries.Product.GetAllProduct
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
    {
        private readonly IRedisRepository<AllProductsModel> _redisProductRepository;

        public GetAllProductQueryHandler(IRedisRepository<AllProductsModel> redisProductRepository)
        {
            _redisProductRepository = redisProductRepository;
        }

        public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
        {
            var memoryProducts = _redisProductRepository.GetAll(Constant.InMemory.InMemoryProductKey, RedisDataType.String).ToList();

            return new(memoryProducts);
        }
    }
}
