namespace AuthenticationService.Application.Features.Command.Authentication.LoginUser
{
    public record LoginUserCommandResponse(
        bool IsSuccess,
        string Token
    );
}
