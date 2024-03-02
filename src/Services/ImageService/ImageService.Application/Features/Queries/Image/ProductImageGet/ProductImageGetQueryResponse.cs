namespace ImageService.Application.Features.Queries.Image
{
    public sealed record ProductImageGetQueryResponse(
        ImageService.Domain.Aggregate.Image Image
      );
}
