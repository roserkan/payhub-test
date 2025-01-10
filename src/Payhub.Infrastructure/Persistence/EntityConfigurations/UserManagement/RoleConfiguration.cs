using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payhub.Domain.Entities.UserManagement;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.UserManagement;

public class RoleConfiguration : BaseEntityConfiguration<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder); 
        builder.ToTable("roles");
        
        builder.Property(i => i.Name).HasColumnName("name").IsRequired();
        builder.Property(i => i.Description).HasColumnName("description");
        builder.Property(i => i.RoleType).HasColumnName("role_type");
        
        // Indexes
        builder.HasIndex(i => i.Name).IsUnique();
    }
}