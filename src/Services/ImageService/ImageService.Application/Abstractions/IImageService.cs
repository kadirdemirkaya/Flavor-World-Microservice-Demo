using BuildingBlock.Base.Models;
using ImageService.Domain.Aggregate;
using ImageService.Domain.Aggregate.Entities;

namespace ImageService.Application.Abstractions
{
    public interface IImageService
    {
        Task<bool> AddImageAsync(FileUpload fileUpload);

        Task<Image> GetImageAsync(Guid imageId);

        Task<bool> AddImageToUserAsync(UserModel user, FileUpload fileUpload);

        Task<bool> AssignUserDefaultImage(UserModel user);

        Task<bool> UpdateImageUserAsync(ImageUser userModel, FileUpload fileUpload);

        Task<bool> AddImageToProduct(ProductModel productModel, FileUpload fileUpload);

        Task<bool> AssignProductDefaultImage(ProductModel productModel);
    }
}
