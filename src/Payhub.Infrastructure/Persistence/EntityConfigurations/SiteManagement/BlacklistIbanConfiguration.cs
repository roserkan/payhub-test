using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payhub.Domain.Entities.AccountManagement;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.BlacklistIbanManagement;

public class BlacklistIbanConfiguration : BaseEntityConfiguration<BlacklistIban>
{
    public override void Configure(EntityTypeBuilder<BlacklistIban> builder)
    {
        base.Configure(builder); 
        builder.ToTable("blacklist_ibans");
        
        builder.Property(i => i.Iban).HasColumnName("iban").IsRequired(); 
        
        // Indexes
        builder.HasIndex(i => i.Iban).IsUnique();
    }
}