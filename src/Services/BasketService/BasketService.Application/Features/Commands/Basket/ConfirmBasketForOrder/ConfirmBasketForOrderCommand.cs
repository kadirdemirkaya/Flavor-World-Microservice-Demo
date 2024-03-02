using MediatR;

namespace BasketService.Application.Features.Commands.Basket.ConfirmBasketForOrder
{
    public record ConfirmBasketForOrderCommand(
        string token
    ) : IRequest<ConfirmBasketForOrderCommandResponse>;
}
