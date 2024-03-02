namespace AuthenticationService.Test.Dtos
{
    public record LoginUserCommandResponse(bool IsSuccess, string Token);
}
