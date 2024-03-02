using ImageService.Domain.Aggregate;
using ImageService.Domain.Aggregate.ValueObjects;
using ImageService.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageService.Infrastructure.Persistence.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable(Constant.TableNames.Images);

            builder.HasKey(i => i.Id);

            builder.Property(m => m.Id)
             .ValueGeneratedNever()
             .HasConversion(
                 id => id.Id,
                 value => ImageId.Create(value));

            builder.Property(i => i.CreatedDate).HasDefaultValue(DateTime.Now);

            builder.Property(i => i.IsActive).HasDefaultValue(true);

            builder.Property(i => i.Name);

            builder.Property(i => i.Path);

            builder.Property(i => i.Photo);

            builder.HasMany(o => o.ImageUsers)
                .WithOne(o => o.Image);
        }
    }
}
