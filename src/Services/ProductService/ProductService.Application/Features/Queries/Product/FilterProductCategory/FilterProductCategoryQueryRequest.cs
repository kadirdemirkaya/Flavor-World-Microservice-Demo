using MediatR;
using ProductService.Domain.Aggregate.ProductAggregate.Enums;

namespace ProductService.Application.Features.Queries.Product.FilterProductCategory
{
    public record FilterProductCategoryQueryRequest(
        ProductCategory ProductCategory
    ) : IRequest<FilterProductCategoryQueryResponse>;
}
