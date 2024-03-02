using BuildingBlock.Base.Models.Base;

namespace ImageService.Domain.Aggregate.ValueObjects
{
    public sealed class ImageId : ValueObject
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public ImageId(Guid id)
        {
            Id = id;
        }

        public static ImageId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static ImageId Create(Guid Id)
        {
            return new ImageId(Id);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
