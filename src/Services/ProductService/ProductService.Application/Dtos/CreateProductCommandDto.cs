using ProductService.Domain.Aggregate.ProductAggregate.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProductService.Application.Dtos
{
    public class CreateProductCommandDto
    {
        [Required(ErrorMessage = "ProductName not null")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Description not null")]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public ProductStatus ProductStatus { get; set; } = ProductStatus.Active;
        public ProductCategory ProductCategory { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
