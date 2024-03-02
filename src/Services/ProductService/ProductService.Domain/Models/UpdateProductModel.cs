using ProductService.Domain.Aggregate.ProductAggregate.Enums;

namespace ProductService.Domain.Models
{
    public class UpdateProductModel
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public double Price { get; set; }
        public int StockCount { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
