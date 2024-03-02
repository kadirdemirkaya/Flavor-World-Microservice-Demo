using BuildingBlock.Base.Models;

namespace AuthenticationService.Application.Abstractions
{
    public interface IUserInfoService
    {
        Task<UserModel> GetUserModel(string token);
    }
}
