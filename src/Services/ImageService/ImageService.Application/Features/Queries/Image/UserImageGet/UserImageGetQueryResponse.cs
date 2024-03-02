namespace ImageService.Application.Features.Queries.Image.UserImageGet
{
    public sealed record UserImageGetQueryResponse(
      ImageService.Domain.Aggregate.Image Image
    );
}
