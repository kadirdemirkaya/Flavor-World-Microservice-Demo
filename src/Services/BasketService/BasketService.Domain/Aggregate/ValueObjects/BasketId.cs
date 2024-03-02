using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Models.Base;

namespace BasketService.Domain.Aggregate.ValueObjects
{
    public sealed class BasketId : ValueObject
    {
        public Guid Id { get; private set; }

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
