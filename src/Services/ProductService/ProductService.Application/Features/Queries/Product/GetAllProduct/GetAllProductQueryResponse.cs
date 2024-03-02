using ProductService.Domain.Models;

namespace ProductService.Application.Features.Queries.Product.GetAllProduct
{
    public record GetAllProductQueryResponse(
      List<AllProductsModel>? AllProductModel
    );
}
