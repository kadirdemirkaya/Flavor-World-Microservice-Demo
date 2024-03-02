using AuthenticationService.Domain.Aggregate;
using AuthenticationService.Domain.Aggregate.ValueObjects;
using AuthenticationService.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthenticationService.Infrastructure.Persistence.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(Constant.TableNames.Users);

            builder.HasKey(o => o.Id);

            builder.Property(m => m.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Id,
                    value => UserId.Create(value));

            builder.HasMany(o => o.RoleUsers)
                   .WithOne(o => o.User);

            builder.Property(o => o.CreatedDate);

            builder.Property(o => o.Email);

            builder.Property(o => o.FullName);

            builder.Property(o => o.Password);

            builder.Property(o => o.UserStatus);

            builder.Property(o => o.RefreshToken);

            builder.Property(o => o.RefreshTokenEndDate);
        }
    }
}
