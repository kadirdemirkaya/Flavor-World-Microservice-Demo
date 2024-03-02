using AuthenticationService.Application.Dtos;
using MediatR;

namespace AuthenticationService.Application.Features.Command.Authentication.CreateUser
{
    public sealed record CreateUserCommandRequest(
       CreateUserDto CreateUserDto
    ) : IRequest<CreateUserCommandResponse>;
}
