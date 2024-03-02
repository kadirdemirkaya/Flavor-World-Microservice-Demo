using BuildingBlock.Base.Models;
using ImageService.Application.Dtos;
using MediatR;

namespace ImageService.Application.Features.Commands.Image.ProductImageAdd
{
    public sealed record ProductImageAddCommand(
        ProductModelDto ProductModelDto,
        FileUpload FileUpload
    ) : IRequest<ProductImageAddCommandResponse>;
}
