using BuildingBlock.Base.Models.Base;
using ProductService.Domain.Aggregate.ProductAggregate.Enums;

namespace ProductService.Domain.Models
{
    public class AllProductsModel : RedisModel
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public double Price { get; set; }
        public int StockCount { get; set; }
    }
}
