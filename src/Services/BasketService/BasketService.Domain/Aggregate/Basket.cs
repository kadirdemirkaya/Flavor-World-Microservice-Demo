using BasketService.Domain.Aggregate.Entities;
using BasketService.Domain.Aggregate.Enums;
using BasketService.Domain.Aggregate.ValueObjects;
using BuildingBlock.Base.Models.Base;

namespace BasketService.Domain.Aggregate
{
    public class Basket : AggregateRoot<BasketId>
    {
        public string CreatedDate { get; set; }
        public BasketStatus BasketStatus { get; set; } = BasketStatus.Active;

        public UserId UserId { get; set; }


        private readonly List<BasketItem> _basketItems = new();
        public IReadOnlyCollection<BasketItem> BasketItems => _basketItems.AsReadOnly();


        public Basket()
        {

        }

        public Basket(BasketId Id, string createdDate, BasketStatus basketStatus, UserId userId) : base(Id)
        {
            CreatedDate = createdDate;
            BasketStatus = basketStatus;
            UserId = userId;
        }

        public static Basket Create(string createdDate, bool isActive, BasketStatus basketStatus, UserId userId)
        {
            return new Basket(BasketId.CreateUnique(), createdDate, basketStatus,userId);
        }

        public static Basket Create(BasketId Id, string createdDate, BasketStatus basketStatus, UserId userId)
        {
            return new Basket(Id, createdDate, basketStatus, userId);
        }

        public void AddBasketItem(BasketId basketId, ProductId productId, string createdDate, bool isActive)
        {
            _basketItems.Add(BasketItem.Create(basketId, productId, isActive));
        }
        public void DeleteBasketItem(BasketId basketId, ProductId productId)
        {
            var basketItem = _basketItems.FirstOrDefault(bi => bi.BasketId == basketId && bi.ProductId == productId);
            _basketItems.Remove(basketItem);
        }

        //public void UpdateBasketItem(BasketId basketId, ProductId productId, bool isActive)
        //{
        //    var basketItem = _basketItems.FirstOrDefault(bi => bi.BasketId == basketId && bi.ProductId == productId);
        //    basketItem.ProductId = productId;
        //    basketItem.BasketId = basketId;
        //    basketItem.IsActive = isActive;
        //}

        public int CountBasketItem() => _basketItems.Count();
    }
}
