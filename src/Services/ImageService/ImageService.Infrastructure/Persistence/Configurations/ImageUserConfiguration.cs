using ImageService.Domain.Aggregate.Entities;
using ImageService.Domain.Aggregate.ValueObjects;
using ImageService.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageService.Infrastructure.Persistence.Configurations
{
    public class ImageUserConfiguration : IEntityTypeConfiguration<ImageUser>
    {
        public void Configure(EntityTypeBuilder<ImageUser> builder)
        {
            builder.ToTable(Constant.TableNames.ImageUsers);

            builder.HasKey(iu => iu.Id);

            builder.Property(m => m.Id)
             .ValueGeneratedNever()
             .HasConversion(
                 id => id.Id,
                 value => ImageUserId.Create(value));

            builder.Property(iu => iu.CreatedDate).HasDefaultValue(DateTime.Now);

            builder.Property(iu => iu.IsActive).HasDefaultValue(true);

            builder.Property(m => m.ImageId)
              .ValueGeneratedNever()
              .HasConversion(
                  id => id.Id,
                  value => ImageId.Create(value));

            builder.HasOne(ru => ru.Image)
                  .WithMany(r => r.ImageUsers)
                  .HasForeignKey(ru => ru.ImageId);

            builder.Property(iu => iu.UserId);
        }
    }
}
