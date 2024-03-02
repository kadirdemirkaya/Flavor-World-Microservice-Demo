using ImageService.Domain.Aggregate.Entities;
using ImageService.Domain.Aggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ImageService.Domain.Constants;

namespace ImageService.Infrastructure.Persistence.Configurations
{
    public class ImageProductConfiguration : IEntityTypeConfiguration<ImageProduct>
    {
        public void Configure(EntityTypeBuilder<ImageProduct> builder)
        {
            builder.ToTable(Constant.TableNames.ImageProducts);

            builder.HasKey(iu => iu.Id);

            builder.Property(m => m.Id)
             .ValueGeneratedNever()
             .HasConversion(
                 id => id.Id,
                 value => ImageProductId.Create(value));

            builder.Property(iu => iu.CreatedDate).HasDefaultValue(DateTime.Now);

            builder.Property(iu => iu.IsActive).HasDefaultValue(true);

            builder.Property(m => m.ImageId)
              .ValueGeneratedNever()
              .HasConversion(
                  id => id.Id,
                  value => ImageId.Create(value));

            builder.HasOne(ru => ru.Image)
                  .WithMany(r => r.ImageProducts)
                  .HasForeignKey(ru => ru.ImageId);

            builder.Property(iu => iu.ProductId);
        }
    }
}
