using BuildingBlock.Base.Models.Base;
using ProductService.Domain.Aggregate.ProductAggregate.Enums;

namespace ProductService.Domain.Models
{
    public class ProductElasticModel : ElasticModel
    {
        public string Name { get; set; }
        public double Price { get; set; }

        public ProductCategory ProductCategory { get; set; } = ProductCategory.FastFood;
    }
}
