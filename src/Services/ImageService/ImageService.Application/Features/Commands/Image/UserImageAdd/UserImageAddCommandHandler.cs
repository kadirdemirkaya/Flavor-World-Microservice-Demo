using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Models;
using ImageService.Application.Abstractions;
using MediatR;

namespace ImageService.Application.Features.Commands.Image.UserImageAdd
{
    public class UserImageAddCommandHandler : IRequestHandler<UserImageAddCommand, UserImageAddCommandResponse>
    {
        private readonly IImageService _imageService;
        private readonly ITokenService _tokenService;

        public UserImageAddCommandHandler(IImageService imageService, ITokenService tokenService)
        {
            _imageService = imageService;
            _tokenService = tokenService;
        }

        public async Task<UserImageAddCommandResponse> Handle(UserImageAddCommand request, CancellationToken cancellationToken)
        {
            UserModel? userModel = await _tokenService.GetUserWithTokenAsync(request.token);
            bool dbResponse = await _imageService.AddImageToUserAsync(userModel, request.FileUpload);
            return new(dbResponse);
        }
    }
}
