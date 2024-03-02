using BuildingBlock.Base.Models;
using BuildingBlock.Base.Models.Base;

namespace ProductService.Domain.Models
{
    public class ProductModel : RedisModel
    {
        public string Description { get; set; }
        public List<ProductDetailModel> ProductDetailModels { get; private set; }

        public ProductModel(string description, List<ProductDetailModel> productDetailModels)
        {
            Description = description;
            ProductDetailModels = productDetailModels;
        }
    }
}
