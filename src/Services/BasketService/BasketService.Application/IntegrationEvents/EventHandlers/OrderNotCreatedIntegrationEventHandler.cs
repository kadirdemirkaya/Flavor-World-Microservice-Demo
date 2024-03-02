using BasketService.Application.IntegrationEvents.Events;
using BuildingBlock.Base.Abstractions;

namespace BasketService.Application.IntegrationEvents.EventHandlers
{
    public class OrderNotCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderNotCreatedIntegrationEvent>
    {
        public async Task Handle(OrderNotCreatedIntegrationEvent @event)
        {
            Serilog.Log.Error("Basket Created Process Not SUccesfully !");
        }
    }
}
