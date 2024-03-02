using AuthenticationService.Domain.Aggregate;
using BuildingBlock.Base.Models;
using ImageService.Domain.Aggregate;

namespace AuthenticationService.Application.Abstractions
{
    public interface IImageService
    {
        Task<bool> AddImageAsync(FileUpload fileUpload);

        Task<Image> GetImageAsync(Guid ImageId);

        Task<bool> AddImageToUserAsync(User user, FileUpload fileUpload);

        Task<bool> AssignUserDefaultImage(User user);
    }
}
