using ProductService.Domain.Models;

namespace ProductService.Application.Features.Commands.Product.UpdateProduct
{
    public record UpdateProductCommandResponse (
        UpdateProductModel UpdateProduct
    );
}
