using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payhub.Domain.Entities.UserManagement;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.UserManagement;

public class UserConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder); 
        builder.ToTable("users");
        
        builder.Property(i => i.Name).HasColumnName("name").IsRequired();
        builder.Property(i => i.Username).HasColumnName("username").IsRequired();
        builder.Property(i => i.PasswordHash).HasColumnName("password_hash").IsRequired();
        builder.Property(i => i.PasswordSalt).HasColumnName("password_salt").IsRequired();
        builder.Property(i => i.IsTwoFactorEnabled).HasColumnName("is_two_factor_enabled").IsRequired();
        builder.Property(i => i.TwoFactorSecret).HasColumnName("two_factor_secret");
        builder.Property(i => i.IsDeleted).HasColumnName("is_deleted").IsRequired();
        builder.Property(i => i.FirstPassword).HasColumnName("first_password");
        
        // Indexes
        builder.HasIndex(i => i.Username).IsUnique();
        builder.HasIndex(i => i.Name).IsUnique();
    }
}