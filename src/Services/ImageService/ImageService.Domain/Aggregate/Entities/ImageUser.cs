using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Models.Base;
using ImageService.Domain.Aggregate.ValueObjects;

namespace ImageService.Domain.Aggregate.Entities
{
    public class ImageUser : AggregateRoot<ImageUserId>
    {
        public Guid UserId { get; set; }

        public ImageId ImageId { get; set; }
        public Image Image { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;


        public ImageUser()
        {

        }

        public ImageUser(ImageUserId Id) : base(Id)
        {

        }
        public ImageUser(ImageUserId Id, Guid userId, Guid imageId) : base(Id)
        {
            UserId = userId;
            ImageId = ImageId.Create(imageId);
        }

        public ImageUser(ImageUserId Id, Guid userId, ImageId imageId) : base(Id)
        {
            UserId = userId;
            ImageId = imageId;
        }

        public static ImageUser Create(Guid userId, Guid imageId)
        {
            return new(ImageUserId.CreateUnique(), userId, imageId);
        }

        public static ImageUser Create(Guid imageUserId, Guid userId, Guid imageId)
        {
            return new(ImageUserId.Create(imageUserId), userId, imageId);
        }

        public static ImageUser Create(ImageUserId imageUserId, Guid userId, Guid imageId)
        {
            return new(imageUserId, userId, imageId);
        }
        public void AddUserDomainEvent(IDomainEvent @event)
        {
            AddDomainEvent(@event);
        }

        public void CountDomainEvent() => DomainEvents.Count();
    }
}
