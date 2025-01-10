using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.SiteManagement;

public class InfrastructureConfiguration : BaseEntityConfiguration<Domain.Entities.SiteManagement.Infrastructure>
{
    public override void Configure(EntityTypeBuilder<Domain.Entities.SiteManagement.Infrastructure> builder)
    {
        base.Configure(builder); 
        builder.ToTable("infrastructures");
        
        builder.Property(i => i.Name).HasColumnName("name").IsRequired(); 
        builder.Property(i => i.Address).HasColumnName("address").IsRequired();
        builder.Property(i => i.DepositAddress).HasColumnName("deposit_address").IsRequired();
        builder.Property(i => i.WithdrawAddress).HasColumnName("withdraw_address").IsRequired();
        
        // Indexes
        builder.HasIndex(i => i.Name).IsUnique();
    }
}