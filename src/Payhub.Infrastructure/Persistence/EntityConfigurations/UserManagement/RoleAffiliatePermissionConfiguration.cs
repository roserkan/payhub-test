using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payhub.Domain.Entities.UserManagement;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.UserManagement;

public class RoleAffiliatePermissionConfiguration : BaseEntityConfiguration<RoleAffiliatePermission>
{
    public override void Configure(EntityTypeBuilder<RoleAffiliatePermission> builder)
    {
        base.Configure(builder);
        builder.ToTable("role_affiliate_permissions");

        builder.Property(i => i.RoleId).HasColumnName("role_id").IsRequired(); // FK
        builder.Property(i => i.AffiliateId).HasColumnName("affiliate_id").IsRequired(); // FK
    }
}