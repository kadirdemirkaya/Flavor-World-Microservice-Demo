using BuildingBlock.Base.Events;

namespace OrderService.Application.IntegrationEvents.Events
{
    public class OrderNotCompletedIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public Guid BasketId { get; set; }
        public string Token { get; set; }

        public OrderNotCompletedIntegrationEvent(Guid orderId, Guid userId, Guid basketId, string token)
        {
            OrderId = orderId;
            UserId = userId;
            BasketId = basketId;
            Token = token;
        }
    }
}
