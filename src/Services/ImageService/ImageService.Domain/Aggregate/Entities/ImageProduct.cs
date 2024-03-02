using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Models.Base;
using ImageService.Domain.Aggregate.ValueObjects;

namespace ImageService.Domain.Aggregate.Entities
{
    public class ImageProduct : AggregateRoot<ImageProductId>
    {
        public Guid ProductId { get; set; }

        public ImageId ImageId { get; set; }
        public Image Image { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;


        public ImageProduct()
        {

        }

        public ImageProduct(ImageProductId Id) : base(Id)
        {

        }
        public ImageProduct(ImageProductId Id, Guid productId, Guid imageId) : base(Id)
        {
            ProductId = productId;
            ImageId = ImageId.Create(imageId);
        }

        public ImageProduct(ImageProductId Id, Guid productId, ImageId imageId) : base(Id)
        {
            ProductId = productId;
            ImageId = imageId;
        }

        public static ImageProduct Create(Guid productId, Guid imageId)
        {
            return new(ImageProductId.CreateUnique(), productId, imageId);
        }

        public static ImageProduct Create(Guid imageProductId, Guid productId, Guid imageId)
        {
            return new(ImageProductId.Create(imageProductId), productId, imageId);
        }

        public static ImageProduct Create(ImageProductId imageProductId, Guid productId, Guid imageId)
        {
            return new(imageProductId, productId, imageId);
        }
        public void AddUserDomainEvent(IDomainEvent @event)
        {
            AddDomainEvent(@event);
        }

        public void CountDomainEvent() => DomainEvents.Count();
    }
}
