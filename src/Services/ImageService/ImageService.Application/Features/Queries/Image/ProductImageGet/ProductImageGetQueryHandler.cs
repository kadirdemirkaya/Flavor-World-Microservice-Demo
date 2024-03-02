using BuildingBlock.Base.Abstractions;
using ImageService.Application.Abstractions;
using ImageService.Domain.Aggregate.Entities;
using ImageService.Domain.Aggregate.ValueObjects;
using MediatR;

namespace ImageService.Application.Features.Queries.Image
{
    public class ProductImageGetQueryHandler : IRequestHandler<ProductImageGetQuery, ProductImageGetQueryResponse>
    {
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;
        public ProductImageGetQueryHandler(IImageService imageService, IUnitOfWork unitOfWork)
        {
            _imageService = imageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductImageGetQueryResponse> Handle(ProductImageGetQuery request, CancellationToken cancellationToken)
        {
            ImageProduct? imageProduct = await _unitOfWork.GetReadRepository<ImageProduct, ImageProductId>().GetAsync(ip => ip.ProductId == request.productId);
            Domain.Aggregate.Image? image = await _imageService.GetImageAsync(imageProduct.ImageId.Id);
            return new(image);
        }
    }
}
