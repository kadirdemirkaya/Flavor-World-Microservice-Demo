using BasketService.Domain.Models;
using MediatR;

namespace BasketService.Application.Features.Commands.Basket.ConfirmBasketInCache
{
    public record ConfirmBasketInCacheCommand(
        List<BasketItemModel> BasketItemModels,
        string token
    ) : IRequest<ConfirmBasketInCacheCommandResponse>;
}
