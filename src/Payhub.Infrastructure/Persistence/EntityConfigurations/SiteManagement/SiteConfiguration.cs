using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payhub.Domain.Entities.SiteManagement;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.SiteManagement;

public class SiteConfiguration : BaseEntityConfiguration<Site>
{
    public override void Configure(EntityTypeBuilder<Site> builder)
    {
        base.Configure(builder); 
        builder.ToTable("sites");
        
        builder.Property(i => i.Name).HasColumnName("name").IsRequired(); 
        builder.Property(i => i.Address).HasColumnName("address").IsRequired();
        builder.Property(i => i.InfrastructureId).HasColumnName("infrastructure_id").IsRequired(); // FK
        
        // Indexes
        builder.HasIndex(i => i.Name).IsUnique();
    }
}