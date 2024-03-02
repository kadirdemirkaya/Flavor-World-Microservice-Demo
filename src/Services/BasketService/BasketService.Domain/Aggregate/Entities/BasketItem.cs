using BasketService.Domain.Aggregate.ValueObjects;
using BuildingBlock.Base.Models.Base;

namespace BasketService.Domain.Aggregate.Entities
{
    public class BasketItem : Entity<BasketItemId>
    {
        public BasketId BasketId { get; private set; }
        public Basket Basket { get; private set; }

        public ProductId ProductId { get; private set; }

        public bool IsActive { get; private set; } = true;


        public BasketItem(BasketItemId Id, BasketId basketId, ProductId productId, bool isActive = true) : base(Id)
        {
            BasketId = basketId;
            ProductId = productId;
            IsActive = isActive;
        }

        public static BasketItem Create(BasketId basketId, ProductId productId, bool isActive)
        {
            return new BasketItem(BasketItemId.CreateUnique(), basketId, productId, isActive);
        }

        public static BasketItem Create(BasketItemId Id, BasketId basketId, ProductId productId, bool isActive)
        {
            return new BasketItem(Id, basketId, productId, isActive);
        }
    }
}
