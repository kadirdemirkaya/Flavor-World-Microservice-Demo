using ImageService.Application.Features.Queries.Image.UserImageGet;
using MediatR;

namespace ImageService.Application.Features.Queries.Image
{
    public sealed record ProductImageGetQuery(
        Guid productId
    ) : IRequest<ProductImageGetQueryResponse>;
}
