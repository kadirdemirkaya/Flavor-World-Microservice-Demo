using BuildingBlock.Base.Abstractions;
using MediatR;
using OrderService.Domain.Aggregate.OrderAggregate.ValueObjects;

namespace OrderService.Application.Features.Commands.OrderCompleted
{
    public class OrderCompletedCommandHandler : IRequestHandler<OrderCompletedCommand, OrderCompletedCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderCompletedCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderCompletedCommandResponse> Handle(OrderCompletedCommand request, CancellationToken cancellationToken)
        {
            bool dbResult = await _unitOfWork.GetWriteRepository<OrderService.Domain.Aggregate.OrderAggregate.Order, OrderId>().CreateAsync(OrderService.Domain.Aggregate.OrderAggregate.Order.Create(request.OrderId, request.Description, DateTime.Now, Domain.Aggregate.OrderAggregate.Enums.OrderStatus.Active, BasketId.Create(request.BasketId)));

            if (dbResult)
                return new(await _unitOfWork.SaveChangesAsync() > 0);
            return new(false);
        }
    }
}
