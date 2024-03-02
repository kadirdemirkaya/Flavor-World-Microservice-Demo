using MediatR;

namespace ProductService.Application.Features.Queries.Product.GetAllProduct
{
    public record GetAllProductQueryRequest(
    
    ) : IRequest<GetAllProductQueryResponse>;
}
