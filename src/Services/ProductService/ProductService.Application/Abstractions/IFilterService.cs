using BuildingBlock.Base.Models.Base;
using ProductService.Domain.Aggregate.ProductAggregate;
using ProductService.Domain.Aggregate.ProductAggregate.Enums;
using ProductService.Domain.Models;

namespace ProductService.Application.Abstractions
{
    public interface IFilterService
    {
        Task<List<ProductElasticModel>> CategoryFilterAsync(string indexName, ProductCategory category);
    }
}
