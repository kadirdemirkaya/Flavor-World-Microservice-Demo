using BuildingBlock.Base.Models;

namespace AuthenticationService.Application.Abstractions
{
    public interface IUserImageService
    {
        Task<bool> AddImageToUser(UserModel userModel,BuildingBlock.Base.Models.FileUpload fileUpload);
    }
}
