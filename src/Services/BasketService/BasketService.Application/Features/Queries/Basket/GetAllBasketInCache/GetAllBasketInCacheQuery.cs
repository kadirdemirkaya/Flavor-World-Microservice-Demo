using MediatR;

namespace BasketService.Application.Features.Queries.Basket.GetAllBasketInCache
{
    public record GetAllBasketInCacheQuery(
        Guid basketId
    ) : IRequest<GetAllBasketInCacheQueryResponse>;
}
