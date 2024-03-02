using MediatR;

namespace ImageService.Application.Features.Queries.Image.UserImageGet
{
    public sealed record UserImageGetQuery(
        string token
    ) : IRequest<UserImageGetQueryResponse>;
}
