using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payhub.Domain.Entities.AccountManagement;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.AccountManagement;

public class AccountSiteConfiguration : BaseEntityConfiguration<AccountSite>
{
    public override void Configure(EntityTypeBuilder<AccountSite> builder)
    {
        base.Configure(builder); 
        builder.ToTable("account_sites");
        
        builder.Property(i => i.AccountId).HasColumnName("account_id").IsRequired(); // FK
        builder.Property(i => i.SiteId).HasColumnName("site_id").IsRequired(); // FK
    }
}