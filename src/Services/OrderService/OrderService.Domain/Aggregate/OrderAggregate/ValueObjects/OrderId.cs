using BuildingBlock.Base.Models.Base;

namespace OrderService.Domain.Aggregate.OrderAggregate.ValueObjects
{
    public sealed class OrderId : ValueObject
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public OrderId(Guid id)
        {
            Id = id;
        }

        public static OrderId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static OrderId Create(Guid Id)
        {
            return new OrderId(Id);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
