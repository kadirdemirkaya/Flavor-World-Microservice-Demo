using MediatR;

namespace BasketService.Application.Features.Commands.Basket.RemoveBasketItemInCache
{
    public record  RemoveBasketItemInCacheCommand(
        Guid productId
    ) : IRequest<RemoveBasketItemInCacheCommandResponse>;
}
