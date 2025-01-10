using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payhub.Domain.Entities.SiteManagement;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.SiteManagement;

public class SitePaymentWayConfiguration : BaseEntityConfiguration<SitePaymentWay>
{
    public override void Configure(EntityTypeBuilder<SitePaymentWay> builder)
    {
        base.Configure(builder);
        builder.ToTable("site_payment_ways");
        
        builder.Property(i => i.SiteId).HasColumnName("site_id").IsRequired(); // FK
        builder.Property(i => i.PaymentWayId).HasColumnName("payment_way_id").IsRequired(); // FK
        builder.Property(i => i.ApiKey).HasColumnName("api_key").IsRequired();
        builder.Property(i => i.SecretKey).HasColumnName("secret_key").IsRequired();
        builder.Property(i => i.Commission).HasColumnName("commission").IsRequired();
        builder.Property(i => i.MinBalanceLimit).HasColumnName("min_balance_limit").IsRequired();
        builder.Property(i => i.MaxBalanceLimit).HasColumnName("max_balance_limit").IsRequired();
        builder.Property(i => i.IsActive).HasColumnName("is_active").IsRequired();
        
        // Indexes
        builder.HasIndex(i => i.SiteId);
        builder.HasIndex(i => i.PaymentWayId);
        builder.HasIndex(i => i.ApiKey).IsUnique();
        builder.HasIndex(i => i.SecretKey).IsUnique();
    }
}

