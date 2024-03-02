using MediatR;

namespace AuthenticationService.Application.Features.Queries.Authentication.RefreshToken
{
    public record RefreshTokenQueryRequest(
        string token
    ) : IRequest<RefreshTokenQueryResponse>;
}
