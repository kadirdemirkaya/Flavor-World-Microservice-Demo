using AuthenticationService.Application.Abstractions;
using AuthenticationService.Application.Exceptions;
using AuthenticationService.Domain.Aggregate;
using AuthenticationService.Domain.Aggregate.Entities;
using AuthenticationService.Domain.Aggregate.Enums;
using AuthenticationService.Domain.Aggregate.ValueObjects;
using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;

namespace AuthenticationService.Application.Features.Command.Authentication.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IHashService _hashService;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IReadRepository<RoleUser, RoleUserId> _readUserRoleRepository;
        private readonly IRoleService _roleService;
        public LoginUserCommandHandler(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator, IHashService hashService, IConfiguration configuration, IUserService userService, IReadRepository<RoleUser, RoleUserId> readUserRoleRepository, IRoleService roleService)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
            _hashService = hashService;
            _configuration = configuration;
            _userService = userService;
            _readUserRoleRepository = readUserRoleRepository;
            _roleService = roleService;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            request.LoginUserDto.Password = _hashService.StringHashingEncrypt(request.LoginUserDto.Password);

            bool dbResult = await _unitOfWork.GetReadRepository<AuthenticationService.Domain.Aggregate.User, UserId>().AnyAsync(u => u.Email == request.LoginUserDto.Email && u.Password == request.LoginUserDto.Password);

            if (dbResult is false)
                return new(false, string.Empty);

            var user = await _unitOfWork.GetReadRepository<AuthenticationService.Domain.Aggregate.User, UserId>().GetAsync(u => u.Email == request.LoginUserDto.Email);

            if (user is null)
                throw new RepositoryErrorException(nameof(IReadRepository<User, UserId>), "User is not exists db");

            //var userRole = await _readUserRoleRepository.GetAsync(u => u.UserId == user.Id);

            Role? role = await _roleService.GetUserRole(user.Id);

            Token? token = _jwtTokenGenerator.GenerateTokenWithRole(new() { Email = user.Email, FullName = user.FullName, Id = user.Id.Id }, Enum.GetName(typeof(RoleEnum), role.RoleEnum));

            if (token is null)
                throw new ServiceErrorException("Token value is null !");

            bool servResult = await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, int.Parse(_configuration["JwtSettings:ExpireMinuteRefToken"]));

            if (servResult is false)
                throw new ServiceErrorException(nameof(IUserService), "token refresh error");

            return new(true, token.AccessToken);
        }
    }
}
