using BuildingBlock.Base.Abstractions;
using OrderService.Application.IntegrationEvents.Events;

namespace OrderService.Application.IntegrationEvents.EventHandlers
{
    public class OrderNotCompletedIntegrationEventHandler : IIntegrationEventHandler<OrderNotCompletedIntegrationEvent>
    {
        public async Task Handle(OrderNotCompletedIntegrationEvent @event)
        {
            Serilog.Log.Error("Order Added Process Not SUccesfully !");
        }
    }
}
