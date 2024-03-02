using BuildingBlock.Base.Events;

namespace BasketService.Application.IntegrationEvents.Events
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
