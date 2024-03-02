using BuildingBlock.Base.Abstractions;

namespace AuthenticationService.Domain.Aggregate.Events
{
    public record UserRegisterEvent(
       string email
    ) : IDomainEvent;
}
