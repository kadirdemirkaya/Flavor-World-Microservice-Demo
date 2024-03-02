using ProductService.Domain.Models;

namespace ProductService.Application.Features.Queries.Product.GetProduct
{
    public record GetProductQueryResponse(
        AllProductModel AllProductModel
    );
}
