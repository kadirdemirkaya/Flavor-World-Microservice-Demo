using BuildingBlock.Base.Events;
using BuildingBlock.Base.Models;

namespace ProductService.Application.IntegrationEvents.Events
{
    public class OrderCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }
        public Guid BasketId { get; set; }
        public Guid OrderId { get; set; }
        public string OrderDescription { get; set; }
        public string Token { get; set; }

        public List<ProductDetailModel> ProductDetailModels { get; set; }

        public OrderCreatedIntegrationEvent(Guid userId, Guid basketId, Guid orderId, string orderDescription, List<ProductDetailModel> productDetailModels, string token)
        {
            UserId = userId;
            BasketId = basketId;
            OrderId = orderId;
            OrderDescription = orderDescription;
            ProductDetailModels = productDetailModels;
            Token = token;
        }
    }
}
