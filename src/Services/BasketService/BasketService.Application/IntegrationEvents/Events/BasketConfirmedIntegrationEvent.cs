using BuildingBlock.Base.Events;
using BuildingBlock.Base.Models;

namespace BasketService.Application.IntegrationEvents.Events
{
    public class BasketConfirmedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }
        public Guid BasketId { get; set; }
        public string OrderDescription { get; set; }
        public string Token { get; set; }

        public List<ProductDetailModel> ProductDetailModels { get; set; }


        public BasketConfirmedIntegrationEvent(Guid userId, Guid basketId, string orderDescription, List<ProductDetailModel> productDetailModels, string token)
        {
            UserId = userId;
            BasketId = basketId;
            ProductDetailModels = productDetailModels;
            OrderDescription = orderDescription;
            Token = token;
        }
    }
}
