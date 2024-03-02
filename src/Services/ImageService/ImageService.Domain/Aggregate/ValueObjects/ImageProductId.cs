using BuildingBlock.Base.Models.Base;

namespace ImageService.Domain.Aggregate.ValueObjects
{
    public sealed class ImageProductId : ValueObject
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public ImageProductId(Guid id)
        {
            Id = id;
        }

        public static ImageProductId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static ImageProductId Create(Guid Id)
        {
            return new ImageProductId(Id);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
