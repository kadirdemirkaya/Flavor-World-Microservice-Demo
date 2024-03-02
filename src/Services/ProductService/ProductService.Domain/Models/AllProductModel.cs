using ProductService.Domain.Aggregate.ProductAggregate.Enums;

namespace ProductService.Domain.Models
{
    public class AllProductModel
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public double Price { get; set; }
    }
}
