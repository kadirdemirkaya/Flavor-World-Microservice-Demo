using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Enums;
using BuildingBlock.Redis;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Domain.Aggregate.ProductAggregate;
using ProductService.Domain.Aggregate.ProductAggregate.Events;
using ProductService.Domain.Aggregate.ProductAggregate.ValueObjects;
using ProductService.Domain.Constants;
using ProductService.Domain.Models;

namespace ProductService.Infrastructure.Events
{
    public class UpdateMemoryDataEventHandler : INotificationHandler<UpdateMemoryDataEvent>
    {
        private string indexName;
        private IHttpContextAccessor _httpContextAccessor;

        public UpdateMemoryDataEventHandler(IHttpContextAccessor httpContextAccessor)
        {
            indexName = nameof(ProductElasticModel).ToLower();
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Handle(UpdateMemoryDataEvent notification, CancellationToken cancellationToken)
        {
            HttpContext context = _httpContextAccessor.HttpContext;
            IServiceProvider serviceProvider = context.RequestServices;

            var _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
            var _redisProductRepository = serviceProvider.GetRequiredService<IRedisRepository<AllProductsModel>>();
            var _elasticService = serviceProvider.GetRequiredService<IElastic<ProductElasticModel>>();

            List<Product>? products = await _unitOfWork.GetReadRepository<Product, ProductId>().GetAllAsync();

            _redisProductRepository.Delete(Constant.InMemory.InMemoryProductKey, Constant.InMemory.InMemoryProductKeyId.ToString(), RedisDataType.String);

            foreach (var product in products) 
                _redisProductRepository.Create(Constant.InMemory.InMemoryProductKey,
                    new()
                    {
                        Description = product.Description,
                        Id = product.Id.Id,
                        Price = product.Price,
                        ProductCategory = product.ProductCategory,
                        ProductName = product.ProductName,
                        ProductStatus = product.ProductStatus,
                        StockCount = product.StockCount,
                    }, RedisDataType.String);

            if (await _elasticService.ChekIndexAsync(indexName))
                if (await _elasticService.DeleteIndexAsync(indexName))
                    foreach (var product in products)
                        await _elasticService.InsertAsync(new() { Id = product.Id.Id, Name = product.ProductName, Price = product.Price, ProductCategory = product.ProductCategory });

            if (await _elasticService.CreateIndexAsync(indexName))
                foreach (var product in products)
                    await _elasticService.InsertAsync(new() { Id = product.Id.Id, Name = product.ProductName, Price = product.Price, ProductCategory = product.ProductCategory });
        }
    }
}
