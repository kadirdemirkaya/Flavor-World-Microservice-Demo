using BuildingBlock.Base.Abstractions;

namespace ProductService.Domain.Aggregate.ProductAggregate.Events
{
    public sealed record SendEmailEvent(
        params string[] emails
    ) : IDomainEvent;
}
