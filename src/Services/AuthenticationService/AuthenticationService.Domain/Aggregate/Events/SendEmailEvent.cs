using BuildingBlock.Base.Abstractions;

namespace AuthenticationService.Domain.Aggregate.Events;
public sealed record SendEmailEvent(
    params string[] emails
) : IDomainEvent;
