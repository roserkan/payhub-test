using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payhub.Domain.Entities.CustomerManagement;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.CustomerManagement;

public class BlacklistConfiguration : BaseEntityConfiguration<Blacklist>
{
    public override void Configure(EntityTypeBuilder<Blacklist> builder)
    {
        base.Configure(builder); 
        builder.ToTable("blacklists");
        
        builder.Property(i => i.BlacklistType).HasColumnName("blacklist_type").IsRequired();
        builder.Property(i => i.Value).HasColumnName("value").IsRequired();
    }
}