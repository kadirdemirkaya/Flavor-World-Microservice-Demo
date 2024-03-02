using MediatR;
using ProductService.Domain.Aggregate.ProductAggregate.Events;

public class SendEmailEventHandler : INotificationHandler<SendEmailEvent>
{
    public async Task Handle(SendEmailEvent notification, CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync("---");
    }
}
