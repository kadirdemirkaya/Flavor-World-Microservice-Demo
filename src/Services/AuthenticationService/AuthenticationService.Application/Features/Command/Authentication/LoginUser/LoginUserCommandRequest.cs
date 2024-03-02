using AuthenticationService.Application.Dtos;
using MediatR;

namespace AuthenticationService.Application.Features.Command.Authentication.LoginUser
{
    public record LoginUserCommandRequest(
        LoginUserDto LoginUserDto
    ) : IRequest<LoginUserCommandResponse>;
}
