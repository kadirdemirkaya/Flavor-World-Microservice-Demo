using BasketService.Domain.Aggregate;
using BasketService.Domain.Aggregate.ValueObjects;
using BasketService.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketService.Infrastructure.Persistence.Configurations
{
    public class BasketConfigurations : IEntityTypeConfiguration<Basket>
    {
        public void Configure(EntityTypeBuilder<Basket> builder)
        {
            builder.ToTable(Constant.TableNames.Baskets);

            builder.HasKey(o => o.Id);

            builder.Property(m => m.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Id,
                    value => BasketId.Create(value));

            builder.Property(m => m.UserId)
               .ValueGeneratedNever()
               .HasConversion(
                   id => id.Id,
                   value => UserId.Create(value));

            builder.HasMany(b => b.BasketItems)
                   .WithOne(b => b.Basket);

            builder.Property(o => o.BasketStatus);

            builder.Property(o => o.CreatedDate);
        }
    }
}
