using BuildingBlock.Redis;
using MediatR;
using ProductService.Domain.Models;
using ProductService.Domain.Constants;
using BuildingBlock.Base.Enums;
using AutoMapper;

namespace ProductService.Application.Features.Queries.Product.GetProduct
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQueryRequest, GetProductQueryResponse>
    {
        private readonly IRedisRepository<AllProductsModel> _redisProductRepository;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(IRedisRepository<AllProductsModel> redisProductRepository, IMapper mapper)
        {
            _redisProductRepository = redisProductRepository;
            _mapper = mapper;
        }

        public async Task<GetProductQueryResponse> Handle(GetProductQueryRequest request, CancellationToken cancellationToken)
        {
            var memoryProducts = _redisProductRepository.GetAll(Constant.InMemory.InMemoryProductKey, RedisDataType.String).ToList();

            return new(_mapper.Map<AllProductModel>(memoryProducts.Find(p => p.Id == request.productId)));
        }
    }
}
