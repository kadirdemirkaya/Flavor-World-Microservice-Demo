﻿using AuthenticationService.Domain.Aggregate;
using BuildingBlock.Base.Models;

namespace AuthenticationService.Application.Abstractions
{
    public interface IUserService
    {
        Task<bool> UpdateRefreshTokenAsync(string refreshToken, User User, DateTime accessTokenDate, int refreshTokenLifeTimeSecond);

        Task<Token> RefreshTokenLoginAsync(string refreshToken);

        Task<UserModel?> ConfirmByEmailAsync(string token, string email);

        Task<UserModel?> GetUserByEmailAsync(string email);

        Task<bool> ResetPasswordAsync(UserModel userModel, string token, string password);
    }
}
