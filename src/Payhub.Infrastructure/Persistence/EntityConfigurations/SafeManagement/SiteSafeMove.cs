using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payhub.Domain.Entities.SafeManagement;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.SafeManagement;

public class SiteSafeMoveConfiguration : BaseEntityConfiguration<SiteSafeMove>
{
    public override void Configure(EntityTypeBuilder<SiteSafeMove> builder)
    {
        base.Configure(builder); 
        builder.ToTable("site_safe_moves");
        
        builder.Property(i => i.SiteId).HasColumnName("site_id").IsRequired();
        builder.Property(i => i.MoveType).HasColumnName("move_type").IsRequired();
        builder.Property(i => i.TransactionType).HasColumnName("transaction_type").IsRequired();
        builder.Property(i => i.Amount).HasColumnName("amount").HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(i => i.CommissionRate).HasColumnName("commission_rate").HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(i => i.CommissionAmount).HasColumnName("commission_amount").HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(i => i.Description).HasColumnName("description").HasMaxLength(500);
        builder.Property(i => i.TransactionDate).HasColumnName("transaction_date").HasColumnType("timestamp without time zone").IsRequired();
    }
} 