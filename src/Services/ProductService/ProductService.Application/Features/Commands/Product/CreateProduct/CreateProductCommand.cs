using MediatR;
using ProductService.Domain.Aggregate.ProductAggregate.Enums;
using ProductService.Domain.Aggregate.ProductAggregate.ValueObjects;

namespace ProductService.Application.Features.Commands.Product.CreateProduct
{
    public record CreateProductCommand(
         string ProductName,
         string Description,
         DateTime CreatedDate,
         ProductStatus ProductStatus,
         ProductCategory ProductCategory,
         decimal Price,
         int StockQuantity
    ) : IRequest<bool>;
}
