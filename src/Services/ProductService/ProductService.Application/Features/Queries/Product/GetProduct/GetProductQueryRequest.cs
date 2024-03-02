using MediatR;

namespace ProductService.Application.Features.Queries.Product.GetProduct
{
    public record GetProductQueryRequest(
        Guid productId
    ) : IRequest<GetProductQueryResponse>;
}
