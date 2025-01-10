using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payhub.Domain.Entities.UserManagement;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.UserManagement;

public class RoleSitePermissionConfiguration : BaseEntityConfiguration<RoleSitePermission>
{
    public override void Configure(EntityTypeBuilder<RoleSitePermission> builder)
    {
        base.Configure(builder);
        builder.ToTable("role_site_permissions");

        builder.Property(i => i.RoleId).HasColumnName("role_id").IsRequired(); // FK
        builder.Property(i => i.SiteId).HasColumnName("site_id").IsRequired(); // FK
    }
}