using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payhub.Domain.Entities.UserManagement;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.UserManagement;

public class SystemPermissionConfiguration : BaseEntityConfiguration<SystemPermission>
{
    public override void Configure(EntityTypeBuilder<SystemPermission> builder)
    {
        base.Configure(builder); 
        builder.ToTable("system_permissions");
        
        builder.Property(i => i.Name).HasColumnName("name").IsRequired();
        builder.Property(i => i.Key).HasColumnName("key").IsRequired();
        builder.Property(i => i.PermissionGroup).HasColumnName("permission_group").IsRequired();
        builder.Property(i => i.Description).HasColumnName("description");
            
        // Indexes
        builder.HasIndex(i => i.Key).IsUnique();
    }
}