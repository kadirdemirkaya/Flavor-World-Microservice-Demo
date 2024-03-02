using BuildingBlock.Base.Models;
using MediatR;

namespace ImageService.Application.Features.Commands.Image.ImageAdd
{
    public sealed record ImageAddCommand(
        FileUpload FileUpload
    ) : IRequest<ImageAddCommandResponse>;
}
