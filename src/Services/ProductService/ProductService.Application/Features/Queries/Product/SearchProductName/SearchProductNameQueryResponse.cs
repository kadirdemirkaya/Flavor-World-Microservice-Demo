using ProductService.Domain.Models;

namespace ProductService.Application.Features.Queries.Product.SearchProductName
{
    public record SearchProductNameQueryResponse(
        List<ProductElasticModel> AllProductModels
    );
}
