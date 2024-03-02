using BuildingBlock.Base.Models;
using MediatR;

namespace ImageService.Application.Features.Commands.Image.UserImageAdd
{
    public sealed record UserImageAddCommand(
        FileUpload FileUpload,
        string token
    ) : IRequest<UserImageAddCommandResponse>;
}
