using BasketService.Application.Dtos;

namespace BasketService.Application.Features.Queries.Basket.GetBasketItem
{
    public record GetBasketItemQueryResponse(
        BasketItemDto BasketItemDto
    );
}
