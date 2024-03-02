using BuildingBlock.Base.Abstractions;
using ImageService.Application.Abstractions;
using MediatR;

namespace ImageService.Application.Features.Commands.Image.ImageAdd
{
    public class ImageAddCommandHandler : IRequestHandler<ImageAddCommand, ImageAddCommandResponse>
    {
        private readonly IImageService _imageService;

        public ImageAddCommandHandler(IImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task<ImageAddCommandResponse> Handle(ImageAddCommand request, CancellationToken cancellationToken)
        {
            bool response = await _imageService.AddImageAsync(request.FileUpload);
            return new(response);
        }
    }
}
