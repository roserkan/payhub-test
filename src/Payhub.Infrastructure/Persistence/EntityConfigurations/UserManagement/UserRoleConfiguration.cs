using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payhub.Domain.Entities.UserManagement;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.UserManagement;

public class UserRoleConfiguration : BaseEntityConfiguration<UserRole>
{
    public override void Configure(EntityTypeBuilder<UserRole> builder)
    {
        base.Configure(builder); 
        builder.ToTable("user_roles");
        
        builder.Property(i => i.UserId).HasColumnName("user_id").IsRequired(); // FK
        builder.Property(i => i.RoleId).HasColumnName("role_id").IsRequired(); // FK
    }
}