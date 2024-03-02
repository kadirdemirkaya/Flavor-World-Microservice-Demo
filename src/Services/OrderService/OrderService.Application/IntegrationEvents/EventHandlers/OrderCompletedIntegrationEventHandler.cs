using BuildingBlock.Base.Abstractions;
using BuildingBlock.Redis;
using MediatR;
using OrderService.Application.Features.Commands.OrderCompleted;
using OrderService.Application.IntegrationEvents.Events;
using OrderService.Domain.Aggregate.OrderAggregate.ValueObjects;
using OrderService.Domain.Models;

namespace OrderService.Application.IntegrationEvents.EventHandlers
{
    public class OrderCompletedIntegrationEventHandler : IIntegrationEventHandler<OrderCompletedIntegrationEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisRepository<OrderModel> _redisOrderRepository;
        private readonly IMediator _mediator;
        public OrderCompletedIntegrationEventHandler(IUnitOfWork unitOfWork, IRedisRepository<OrderModel> redisOrderRepository, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _redisOrderRepository = redisOrderRepository;
            _mediator = mediator;
        }

        public async Task Handle(OrderCompletedIntegrationEvent @event)
        {
            var orderModels = await _redisOrderRepository.GetByIdAsync(OrderService.Domain.Constants.Constant.InMemory.InMemoryOrderKey, OrderService.Domain.Constants.Constant.InMemory.InMemoryOrderKeyId.ToString(), BuildingBlock.Base.Enums.RedisDataType.String);

            OrderCompletedCommand orderCompleted = new(orderModels.OrderId, orderModels.BasketId, orderModels.Description);

            OrderCompletedCommandResponse response = await _mediator.Send(orderCompleted);

            if (response.Result)
            {
                _redisOrderRepository.Delete(OrderService.Domain.Constants.Constant.InMemory.InMemoryOrderKey, OrderService.Domain.Constants.Constant.InMemory.InMemoryOrderKeyId.ToString(), BuildingBlock.Base.Enums.RedisDataType.String);
                Serilog.Log.Information("Order Created Process Succesfully");
            }
            Serilog.Log.Information("Order Created Process Not Succesfully");
        }
    }
}
