using MediatR;

namespace ProductService.Application.Features.Commands.Product.DeleteProduct
{
    public record DeleteProductCommandRequest(
        Guid ProductId
    ) : IRequest<DeleteProductCommandResponse>;
}
