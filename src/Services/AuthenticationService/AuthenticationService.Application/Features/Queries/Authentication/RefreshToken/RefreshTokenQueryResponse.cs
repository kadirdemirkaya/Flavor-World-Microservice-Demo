namespace AuthenticationService.Application.Features.Queries.Authentication.RefreshToken
{
    public record RefreshTokenQueryResponse(
        string newToken
    );
}
