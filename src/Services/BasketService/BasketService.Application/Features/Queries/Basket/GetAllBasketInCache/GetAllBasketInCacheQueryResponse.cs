using MediatR;

namespace BasketService.Application.Features.Queries.Basket.GetAllBasketInCache
{
    public record GetAllBasketInCacheQueryResponse(
        List<Domain.Aggregate.Entities.BasketItem> BasketItems
    );
}
