using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payhub.Domain.Entities.AccountManagement;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.AccountManagement;

public class BankConfiguration : BaseEntityConfiguration<Bank>
{
    public override void Configure(EntityTypeBuilder<Bank> builder)
    {
        base.Configure(builder); 
        builder.ToTable("banks");
        
        builder.Property(i => i.Name).HasColumnName("name").IsRequired();
        builder.Property(i => i.IconUrl).HasColumnName("icon_url").IsRequired();
        
        // Indexes
        builder.HasIndex(i => i.Name).IsUnique();
    }
}
