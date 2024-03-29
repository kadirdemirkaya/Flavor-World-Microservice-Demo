﻿using AuthenticationService.Application.Abstractions;
using AuthenticationService.Application.Exceptions;
using AuthenticationService.Domain.Aggregate;
using AuthenticationService.Domain.Aggregate.ValueObjects;
using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Models;
using MediatR;

namespace AuthenticationService.Application.Features.Queries.Authentication.RefreshToken
{
    public class RefreshTokenQueryHandler : IRequestHandler<RefreshTokenQueryRequest, RefreshTokenQueryResponse>
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public RefreshTokenQueryHandler(IUserService userService, IJwtTokenGenerator jwtTokenGenerator, IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _userService = userService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<RefreshTokenQueryResponse> Handle(RefreshTokenQueryRequest request, CancellationToken cancellationToken)
        {
            string email = _tokenService.GetEmailWithToken(request.token);
            User? user = await _unitOfWork.GetReadRepository<User, UserId>().GetAsync(u => u.Email == email);
            if (user is null)
                throw new UserNotExistsException();
            Token? token = await _userService.RefreshTokenLoginAsync(user.RefreshToken);
            if (token is null)
                throw new ServiceErrorException(nameof(ITokenService), "Token value is null !");
            return new(token.AccessToken);
        }
    }
}
