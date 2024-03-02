using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Models.Base;
using ImageService.Domain.Aggregate.Entities;
using ImageService.Domain.Aggregate.Enums;
using ImageService.Domain.Aggregate.ValueObjects;

namespace ImageService.Domain.Aggregate
{
    public class Image : AggregateRoot<ImageId>
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public byte[] Photo { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public FileType FileType { get; set; } = FileType.image;
        public ContentType ContentType { get; set; } = ContentType.jpeg;


        private readonly List<ImageUser>? _imageUsers = new();
        public IReadOnlyCollection<ImageUser> ImageUsers => _imageUsers.AsReadOnly();


        private readonly List<ImageProduct>? _imageProducts = new();
        public IReadOnlyCollection<ImageProduct> ImageProducts => _imageProducts.AsReadOnly();


        public Image()
        {

        }
        public Image(ImageId imageId) : base(imageId)
        {

        }
        public Image(ImageId imageId, string name, string path, byte[] photo, FileType fileType, ContentType contentType) : base(imageId)
        {
            Name = name;
            Path = path;
            Photo = photo;
            FileType = fileType;
            ContentType = contentType;
        }

        public static Image Create(string name, string path, byte[] photo, FileType fileType, ContentType contentType)
        {
            return new(ImageId.CreateUnique(), name, path, photo, fileType, contentType);
        }

        public static Image Create(Guid id, string name, string path, byte[] photo, FileType fileType, ContentType contentType)
        {
            return new(ImageId.Create(id), name, path, photo, fileType, contentType);
        }

        public static Image Create(ImageId imageId, string name, string path, byte[] photo, FileType fileType, ContentType contentType)
        {
            return new(imageId, name, path, photo, fileType, contentType);
        }

        public void AddImageUser(Guid UserId, Guid ImageId)
        {
            _imageUsers.Add(ImageUser.Create(UserId, ImageId));
        }

        public void AddImageUser(ImageUser imageUser)
        {
            _imageUsers.Add(imageUser);
        }

        public void AddImageProduct(Guid productId, Guid imageId)
        {
            _imageProducts.Add(ImageProduct.Create(productId,imageId));
        }

        public void AddImageProduct(ImageProduct imageUser)
        {
            _imageProducts.Add(imageUser);
        }


        public int ImageUserCount() => ImageUsers.Count();
        public int ImageProductCount() => ImageProducts.Count();

        public void AddUserDomainEvent(IDomainEvent @event)
        {
            AddDomainEvent(@event);
        }

        public void CountDomainEvent() => DomainEvents.Count();
    }
}
