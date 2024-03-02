using AuthenticationService.Application.Abstractions;
using AuthenticationService.Infrastructure.Persistence.Data;
using BuildingBlock.Base.Abstractions;
using MediatR;

namespace AuthenticationService.Infrastructure.Services
{
    public class PubEventService : IPubEventService
    {
        private static AuthenticationDbContext _dbContext;
        private readonly IMediator _mediator;

        public PubEventService(AuthenticationDbContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public AuthenticationDbContext GetContext() => _dbContext;

        public async Task PublishDomainEventAsync(string serviceName)
        {
            if (serviceName == AuthenticationService.Domain.Constants.Constant.App.ApplicationName)
            {
                var context = GetContext();
                if (context is null)
                    return;

                var entityDomainEvents = GetHasDomainEvent(context);

                var domainEvents = GetDomainEvent(entityDomainEvents);

                ClearDomainEvent(entityDomainEvents);

                await PublishToDomainEvents(domainEvents);
            }
        }

        private List<IHasDomainEvent> GetHasDomainEvent(AuthenticationDbContext context)
            => context.ChangeTracker.Entries<IHasDomainEvent>()
                   .Where(entry => entry.Entity.DomainEvents.Any())
                   .Select(entry => entry.Entity).ToList();

        private List<IDomainEvent> GetDomainEvent(List<IHasDomainEvent> hasDomainEvents)
            => hasDomainEvents.SelectMany(entry => entry.DomainEvents).ToList();

        private void ClearDomainEvent(List<IHasDomainEvent> hasDomainEvents)
             => hasDomainEvents.ForEach(entity => entity.ClearDomainEvents());

        private async Task PublishToDomainEvents(List<IDomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent);
            }
        }
    }
}
