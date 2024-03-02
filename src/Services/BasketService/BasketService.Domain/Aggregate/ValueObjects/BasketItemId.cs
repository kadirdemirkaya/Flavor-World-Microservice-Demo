using BuildingBlock.Base.Models.Base;

namespace BasketService.Domain.Aggregate.ValueObjects
{
    public sealed class BasketItemId : ValueObject
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public BasketItemId(Guid id)
        {
            Id = id;
        }

        public static BasketItemId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static BasketItemId Create(Guid Id)
        {
            return new BasketItemId(Id);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
