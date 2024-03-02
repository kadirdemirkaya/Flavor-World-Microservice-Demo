using BasketService.Application.Dtos;
using MediatR;

namespace BasketService.Application.Features.Queries.Basket.GetBasketItem
{
    public record GetBasketItemQuery(
        Guid ProductId
    ) : IRequest<GetBasketItemQueryResponse>;
}
