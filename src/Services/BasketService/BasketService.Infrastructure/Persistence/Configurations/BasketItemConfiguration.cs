using BasketService.Domain.Aggregate.Entities;
using BasketService.Domain.Aggregate.ValueObjects;
using BasketService.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketService.Infrastructure.Persistence.Configurations
{
    public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder.ToTable(Constant.TableNames.BasketItems);

            builder.HasKey(b => b.Id);

            builder.Property(m => m.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Id,
                    value => BasketItemId.Create(value));

            builder.HasOne(i => i.Basket)
                   .WithMany(i => i.BasketItems)
                   .HasForeignKey(i => i.BasketId);

            builder.Property(m => m.BasketId)
               .ValueGeneratedNever()
               .HasConversion(
                   id => id.Id,
                   value => BasketId.Create(value));

            builder.Property(m => m.ProductId)
               .ValueGeneratedNever()
               .HasConversion(
                   id => id.Id,
                   value => ProductId.Create(value));

            builder.Property(b => b.IsActive);
        }
    }
}
