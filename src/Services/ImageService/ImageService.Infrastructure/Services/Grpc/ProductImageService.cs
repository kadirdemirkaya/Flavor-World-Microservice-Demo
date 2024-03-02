using Grpc.Core;
using ImageProductService;
using ImageService.Application.Abstractions;

namespace ImageService.Infrastructure.Services.Grpc
{
    public class ProductImageService : GrpcImageProduct.GrpcImageProductBase
    {
        private readonly IImageService _imageService;
        private readonly IImageTypeService _imageTypeService;
        private bool _dbResult;
        public ProductImageService(IImageService imageService, IImageTypeService imageTypeService)
        {
            _imageService = imageService;
            _dbResult = false;
            _imageTypeService = imageTypeService;
        }

        public override async Task<ProductImageAddModelResponse> ProductImageAdd(ProductImageAddModelRequest request, ServerCallContext context)
        {
            if (request.FileUploadModel.Files is not null && request.FileUploadModel.Files.Length > 1)
                _dbResult = await _imageService.AddImageToProduct(new() { ProductId = Guid.Parse(request.ProductModel.ProductId) }, new()
                {
                    File = _imageTypeService.ConvertToIFormFile(request.FileUploadModel.Files.ToArray(), request.FileUploadModel.Name),
                    Name = request.FileUploadModel.Name,
                    Path = request.FileUploadModel.Path
                });

            _dbResult = await _imageService.AssignProductDefaultImage(new() { ProductId = Guid.Parse(request.ProductModel.ProductId) });

            return new() { Result = _dbResult };
        }
    }
}
