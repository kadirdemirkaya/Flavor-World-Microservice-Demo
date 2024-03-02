using AuthenticationService.Domain.Aggregate;
using AuthenticationService.Domain.Aggregate.ValueObjects;
using AuthenticationUserInfoService;
using BuildingBlock.Base.Abstractions;
using Grpc.Core;

namespace AuthenticationService.Infrastructure.Services.Grpc
{
    public class UserInfoService : GrpcUserInfo.GrpcUserInfoBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public UserInfoService(ITokenService tokenService, IUnitOfWork unitOfWork)
        {
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public override async Task<GetUserModelResponse> GetUserInfo(GetUserModelRequest request, ServerCallContext context)
        {
            string? email = _tokenService.GetEmailWithToken(request.Token);
            User? user = await _unitOfWork.GetReadRepository<User, UserId>().GetAsync(u => u.Email == email);
            return new() { UserModel = new() { Email = user.Email, FullName = user.FullName, Id = user.Id.Id.ToString() } };
        }
    }
}
