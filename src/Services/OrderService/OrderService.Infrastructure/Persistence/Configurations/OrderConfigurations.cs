using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Aggregate.OrderAggregate;
using OrderService.Domain.Aggregate.OrderAggregate.ValueObjects;
using OrderService.Domain.Constants;

namespace OrderService.Infrastructure.Persistence.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable(Constant.TableNames.Orders);

            builder.HasKey(o => o.Id);

            builder.Property(m => m.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Id,
                    value => OrderId.Create(value));

            builder.Property(m => m.BasketId)
               .ValueGeneratedNever()
               .HasConversion(
                   id => id.Id,
                   value => BasketId.Create(value));

            builder.Property(o => o.CreatedDate);

            builder.Property(o => o.OrderStatus);

            builder.Property(o => o.Description);
        }
    }
}
