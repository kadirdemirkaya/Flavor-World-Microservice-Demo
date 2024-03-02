using BuildingBlock.Base.Models.Base;

namespace ImageService.Domain.Aggregate.ValueObjects
{
    public sealed class ImageUserId : ValueObject
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public ImageUserId(Guid id)
        {
            Id = id;
        }

        public static ImageUserId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static ImageUserId Create(Guid Id)
        {
            return new ImageUserId(Id);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
