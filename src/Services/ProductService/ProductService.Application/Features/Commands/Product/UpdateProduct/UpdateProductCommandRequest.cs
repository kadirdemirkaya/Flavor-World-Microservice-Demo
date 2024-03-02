using MediatR;
using ProductService.Domain.Models;

namespace ProductService.Application.Features.Commands.Product.UpdateProduct
{
    public record UpdateProductCommandRequest (
        UpdateProductModel UpdateProductModel
    ) : IRequest<UpdateProductCommandResponse>;
}
