using ProductService.Domain.Models;

namespace ProductService.Application.Features.Queries.Product.FilterProductCategory
{
    public record FilterProductCategoryQueryResponse (
        List<ProductElasticModel> ProductElasticModels
    );
}
