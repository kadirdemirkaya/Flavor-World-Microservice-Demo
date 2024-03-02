using AuthenticationService.Domain.Aggregate.Events;
using MediatR;

namespace AuthenticationService.Infrastructure.Events;

public class SendEmailEventHandler : INotificationHandler<SendEmailEvent>
{
    public async Task Handle(SendEmailEvent notification, CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync("---");
    }
}
