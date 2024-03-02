using ImageService.Application.Abstractions;
using MediatR;

namespace ImageService.Application.Features.Commands.Image.ProductImageAdd
{
    public class ProductImageAddCommandHandler : IRequestHandler<ProductImageAddCommand, ProductImageAddCommandResponse>
    {
        private readonly IImageService _imageService;

        public ProductImageAddCommandHandler(IImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task<ProductImageAddCommandResponse> Handle(ProductImageAddCommand request, CancellationToken cancellationToken)
        {
            bool resultService = await _imageService.AddImageToProduct(new() { ProductId = request.ProductModelDto.ProductId }, request.FileUpload);

            return new(resultService);
        }
    }
}
