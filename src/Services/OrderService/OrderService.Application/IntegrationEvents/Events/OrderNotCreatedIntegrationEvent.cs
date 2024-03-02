using BuildingBlock.Base.Events;
using BuildingBlock.Base.Models;

namespace OrderService.Application.IntegrationEvents.Events
{
    public class OrderNotCreatedIntegrationEvent : IntegrationEvent
    {
        public string Token { get; set; }

        public OrderNotCreatedIntegrationEvent(string token)
        {
            Token = token;
        }
    }
}
