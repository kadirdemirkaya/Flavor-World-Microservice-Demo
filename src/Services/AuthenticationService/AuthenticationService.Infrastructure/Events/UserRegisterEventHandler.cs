using AuthenticationService.Domain.Aggregate.Events;
using MediatR;

namespace AuthenticationService.Application.Events;

public class UserRegisterEventHandler : INotificationHandler<UserRegisterEvent>
{
    public async Task Handle(UserRegisterEvent notification, CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync("---");
    }
}
