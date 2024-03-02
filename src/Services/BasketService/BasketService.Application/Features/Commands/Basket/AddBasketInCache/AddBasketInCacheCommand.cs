using BasketService.Application.Dtos;
using MediatR;

namespace BasketService.Application.Features.Commands.Basket.AddBasketInCache
{
    public record AddBasketInCacheCommand(
        AddBasketDto addBasketDto
    ) : IRequest<AddBasketInCacheCommandResponse>;
}
