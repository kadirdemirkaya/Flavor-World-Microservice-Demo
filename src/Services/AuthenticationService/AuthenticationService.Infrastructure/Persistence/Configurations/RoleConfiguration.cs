using AuthenticationService.Domain.Aggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AuthenticationService.Domain.Constants;
using AuthenticationService.Domain.Aggregate.ValueObjects;

namespace AuthenticationService.Infrastructure.Persistence.Configurations
{
    public class RoleConfigurations : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(Constant.TableNames.Roles);

            builder.HasKey(r => r.Id);

            builder.Property(m => m.Id)
               .ValueGeneratedNever()
               .HasConversion(
                   id => id.Id,
                   value => RoleId.Create(value));

            builder.HasMany(o => o.RoleUsers)
                   .WithOne(o => o.Role);

            builder.Property(r => r.RoleEnum);

            builder.Property(r => r.IsActive);
        }
    }
}
