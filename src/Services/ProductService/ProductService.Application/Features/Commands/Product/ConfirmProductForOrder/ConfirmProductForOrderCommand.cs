using BuildingBlock.Base.Models;
using MediatR;

namespace ProductService.Application.Features.Commands.Product.ConfirmProductForOrder
{
    public record ConfirmProductForOrderCommand(
        ProductService.Domain.Models.ProductModel ProductModels
    ) : IRequest<ConfirmProductForOrderCommandResponse>;
}
