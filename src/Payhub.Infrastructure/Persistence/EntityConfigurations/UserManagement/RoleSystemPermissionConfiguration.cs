using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payhub.Domain.Entities.UserManagement;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.UserManagement;

public class RoleSystemPermissionConfiguration : BaseEntityConfiguration<RoleSystemPermission>
{
    public override void Configure(EntityTypeBuilder<RoleSystemPermission> builder)
    {
        base.Configure(builder);
        builder.ToTable("role_system_permissions");

        builder.Property(i => i.RoleId).HasColumnName("role_id").IsRequired(); // FK
        builder.Property(i => i.SystemPermissionId).HasColumnName("system_permission_id").IsRequired(); // FK
    }
}