using BuildingBlock.Base.Models.Base;

namespace OrderService.Domain.Aggregate.OrderAggregate.ValueObjects
{
    public sealed class BasketId : ValueObject
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public BasketId(Guid id)
        {
            Id = id;
        }

        public static BasketId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static BasketId Create(Guid Id)
        {
            return new BasketId(Id);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
