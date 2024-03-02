using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Aggregate.ProductAggregate;
using ProductService.Domain.Aggregate.ProductAggregate.ValueObjects;
using ProductService.Domain.Constants;

namespace ProductService.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(Constant.TableNames.Products);

            builder.HasKey(p => p.Id);

            builder.Property(m => m.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Id,
                    value => ProductId.Create(value));

            builder.Property(p => p.ProductStatus);

            builder.Property(p => p.ProductCategory);

            builder.Property(p => p.CreatedDate);

            builder.Property(p => p.Description);

            builder.Property(p => p.ProductName);

            builder.Property(p => p.StockCount);

            builder.Property(p => p.Price);

            builder.OwnsOne(p => p.ProductDetail, pd =>
            {
                pd.Property(d => d.Price).HasColumnName("ProductDetail_Price");
                pd.Property(d => d.StockQuantity).HasColumnName("ProductDetail_StockQuantity");
            });
        }
    }
}
