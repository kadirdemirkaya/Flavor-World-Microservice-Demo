using BuildingBlock.Base.Abstractions;

namespace ProductService.Domain.Aggregate.ProductAggregate.Events
{
    public sealed record UpdateMemoryDataEvent(
        params string[] tableNames
    ) : IDomainEvent;
}
