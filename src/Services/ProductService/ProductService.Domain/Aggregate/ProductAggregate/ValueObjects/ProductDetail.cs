using BuildingBlock.Base.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.Domain.Aggregate.ProductAggregate.ValueObjects
{
    [Owned]
    [NotMapped]
    public sealed class ProductDetail : ValueObject
    {
        public decimal Price { get; private set; }
        public int StockQuantity { get; private set; }

        public ProductDetail(decimal price, int stockQuantity)
        {
            Price = price;
            StockQuantity = stockQuantity;
        }

        public static ProductDetail Create(decimal price, int stockQuantity)
        {
            return new(price, stockQuantity);
        }

        public void ProductDetailSet(decimal price, int stockQuantity)
        {
            Price = price;
            StockQuantity = stockQuantity;
        }


        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Price;
            yield return StockQuantity;
        }
    }
}
