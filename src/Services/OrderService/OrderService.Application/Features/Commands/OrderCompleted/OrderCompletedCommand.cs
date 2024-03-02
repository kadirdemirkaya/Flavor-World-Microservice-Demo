using MediatR;

namespace OrderService.Application.Features.Commands.OrderCompleted
{
    public record OrderCompletedCommand(
        Guid OrderId,
        Guid BasketId,
        string Description
    ) : IRequest<OrderCompletedCommandResponse>;
}
