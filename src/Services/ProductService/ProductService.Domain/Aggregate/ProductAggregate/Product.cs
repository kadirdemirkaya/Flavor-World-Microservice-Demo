using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Models.Base;
using ProductService.Domain.Aggregate.ProductAggregate.Enums;
using ProductService.Domain.Aggregate.ProductAggregate.ValueObjects;

namespace ProductService.Domain.Aggregate.ProductAggregate
{
    public class Product : AggregateRoot<ProductId>
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int StockCount { get; set; }
        public double Price { get; set; }
        public ProductStatus ProductStatus { get; set; } = ProductStatus.Active;
        public ProductCategory ProductCategory { get; set; }
        public ProductDetail ProductDetail { get; private set; }

        public Product(ProductId Id, string productName, string description, DateTime createdDate, int stockCount, double price, ProductStatus productStatus, ProductCategory productCategory, ProductDetail productDetail) : base(Id)
        {
            ProductName = productName;
            Description = description;
            CreatedDate = createdDate;
            ProductStatus = productStatus;
            ProductDetail = productDetail;
            StockCount = stockCount;
            Price = price;
            ProductCategory = productCategory;
        }

        public Product()
        {

        }

        public Product(ProductId Id) : base(Id)
        {

        }

        public static Product Create(ProductId id)
        {
            return new(id, default, default, default, default, default, default, default, default);
        }

        public static Product Create(Guid id)
        {
            return new(ProductId.Create(id), default, default, default, default, default, default, default, default);
        }
        public static Product Create(string productName, string description, DateTime createdDate, ProductStatus productStatus, int stockCount, decimal price, ProductCategory productCategory, ProductDetail productDetail)
        {
            return new(ProductId.CreateUnique(), productName, description, createdDate, stockCount, Convert.ToDouble(price), productStatus, productCategory, productDetail);
        }

        public static Product Create(ProductId Id, string productName, string description, DateTime createdDate, int stockCount, decimal price, ProductStatus productStatus, ProductCategory productCategory, ProductDetail productDetail)
        {
            return new(Id, productName, description, createdDate, stockCount, Convert.ToDouble(price), productStatus, productCategory, productDetail);
        }

        public void AddProductDomainEvent(IDomainEvent @event)
        {
            AddDomainEvent(@event);
        }
    }
}
