using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Models.Base;
using OrderService.Domain.Aggregate.OrderAggregate.Enums;
using OrderService.Domain.Aggregate.OrderAggregate.ValueObjects;

namespace OrderService.Domain.Aggregate.OrderAggregate
{
    public class Order : AggregateRoot<OrderId>
    {
        public string Description { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Active;
        public BasketId BasketId { get; set; }

        public Order()
        {
            
        }
        public Order(OrderId Id) : base(Id)
        {

        }
        public Order(OrderId id, string description, DateTime createdDate, OrderStatus orderStatus, BasketId basketId) : base(id)
        {
            Description = description;
            CreatedDate = createdDate;
            OrderStatus = orderStatus;
            BasketId = basketId;
        }

        public static Order Create(string description, DateTime createdDate, OrderStatus orderStatus, BasketId basketId)
        {
            return new(OrderId.CreateUnique(), description, createdDate, orderStatus,basketId);
        }

        public static Order Create(Guid id, string description, DateTime createdDate, OrderStatus orderStatus, BasketId basketId)
        {
            return new(OrderId.Create(id), description, createdDate, orderStatus,basketId);
        }

        public void AddOrderDomainEvent(IDomainEvent @event)
        {
            AddDomainEvent(@event);
        }
    }
}
