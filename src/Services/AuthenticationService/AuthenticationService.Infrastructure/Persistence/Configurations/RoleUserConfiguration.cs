using AuthenticationService.Domain.Aggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AuthenticationService.Domain.Constants;
using AuthenticationService.Domain.Aggregate.ValueObjects;

namespace AuthenticationService.Infrastructure.Persistence.Configurations
{
    public class RoleUserConfiguration : IEntityTypeConfiguration<RoleUser>
    {
        public void Configure(EntityTypeBuilder<RoleUser> builder)
        {
            builder.ToTable(Constant.TableNames.RolUsers);

            builder.HasKey(r => r.Id);

            builder.Property(m => m.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Id,
                value => RoleUserId.Create(value));

            builder.Property(m => m.RoleId)
           .ValueGeneratedNever()
           .HasConversion(
               id => id.Id,
               value => RoleId.Create(value));

            builder.Property(m => m.UserId)
           .ValueGeneratedNever()
           .HasConversion(
               id => id.Id,
               value => UserId.Create(value));

            builder.Property(r => r.RoleId);

            builder.Property(r => r.UserId);

            builder.HasOne(ru => ru.Role)
                   .WithMany(r => r.RoleUsers)
                   .HasForeignKey(ru => ru.RoleId);

            builder.HasOne(ru => ru.User)
                   .WithMany(r => r.RoleUsers)
                   .HasForeignKey(ru => ru.UserId);

            builder.Property(r => r.UserRoleStatus);

            builder.Property(r => r.IsActive);
        }
    }
}
