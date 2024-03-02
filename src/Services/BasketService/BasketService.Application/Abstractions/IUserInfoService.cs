using BuildingBlock.Base.Models;

namespace BasketService.Application.Abstractions
{
    public interface IUserInfoService
    {
        Task<UserModel> GetUserModel(string token);
    }
}
