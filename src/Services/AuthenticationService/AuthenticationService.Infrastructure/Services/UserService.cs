using AuthenticationService.Application.Abstractions;
using AuthenticationService.Domain.Aggregate;
using AuthenticationService.Domain.Aggregate.ValueObjects;
using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Models;
using Elastic.CommonSchema;
using Microsoft.Extensions.Configuration;

namespace AuthenticationService.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator, IConfiguration configuration, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public async Task<UserModel?> ConfirmByEmailAsync(string token, string email)
        {
            string _email = _tokenService.GetEmailWithToken(token);
            if (_email.Equals(email))
            {
                AuthenticationService.Domain.Aggregate.User? user = await _unitOfWork.GetReadRepository<AuthenticationService.Domain.Aggregate.User, UserId>().GetAsync(u => u.Email == _email);
                return new() { Email = user.Email, FullName = user.FullName, Id = user.Id.Id };
            }
            return null;
        }

        public async Task<UserModel?> GetUserByEmailAsync(string email)
        {
            AuthenticationService.Domain.Aggregate.User user = await _unitOfWork.GetReadRepository<AuthenticationService.Domain.Aggregate.User, UserId>().GetAsync(u => u.Email == email);
            return new() { Email = user.Email, FullName = user.FullName, Id = user.Id.Id };
        }

        public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
        {
            AuthenticationService.Domain.Aggregate.User? user = await _unitOfWork.GetReadRepository<AuthenticationService.Domain.Aggregate.User, UserId>().GetAsync(u => u.RefreshToken == refreshToken);
            if (user is not null && user?.RefreshTokenEndDate > DateTime.Now)
            {
                Token token = _jwtTokenGenerator.GenerateToken(new UserModel() { Email = user.Email, FullName = user.FullName, Id = Guid.Parse(user.Id.Id.ToString()) });
                await UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, int.Parse(_configuration["JwtSettings:ExpireMinuteRefToken"]));
                return token;
            }
            return null;
        }

        public async Task<bool> ResetPasswordAsync(UserModel userModel, string token, string password)
        {
            //to do
            return default;
        }

        public async Task<bool> UpdateRefreshTokenAsync(string refreshToken, AuthenticationService.Domain.Aggregate.User User, DateTime accessTokenDate, int refreshTokenLifeTimeSecond)
        {
            if (User is not null)
            {
                User.RefreshToken = refreshToken;
                User.RefreshTokenEndDate = accessTokenDate.AddMinutes(refreshTokenLifeTimeSecond);

                _unitOfWork.GetWriteRepository<AuthenticationService.Domain.Aggregate.User, UserId>().UpdateAsync(User);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
