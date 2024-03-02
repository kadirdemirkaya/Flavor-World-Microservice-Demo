using BuildingBlock.Base.Models;

namespace ProductService.Application.Abstractions
{
    public interface IProductImageService
    {
        Task<bool> ProductImageAddAsync(BuildingBlock.Base.Models.ProductModel productModel, BuildingBlock.Base.Models.FileUpload fileUpload);
    }
}
