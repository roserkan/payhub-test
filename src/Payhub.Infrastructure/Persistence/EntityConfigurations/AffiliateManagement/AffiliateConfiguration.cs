using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payhub.Domain.Entities;
using Payhub.Domain.Entities.AffiliateManagement;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.AffiliateManagement;


public class AffiliateConfiguration : BaseEntityConfiguration<Affiliate>
{
    public override void Configure(EntityTypeBuilder<Affiliate> builder)
    {
        base.Configure(builder); 
        builder.ToTable("affiliates");
        
        builder.Property(i => i.Name).HasColumnName("name").IsRequired();
        builder.Property(i => i.IsDynamic).HasColumnName("is_dynamic").IsRequired();
        builder.Property(i => i.DailyDepositLimit).HasColumnName("daily_deposit_limit").IsRequired();
        builder.Property(i => i.DailyWithdrawLimit).HasColumnName("daily_withdraw_limit").IsRequired();
        builder.Property(i => i.MinDepositAmount).HasColumnName("min_deposit_amount").IsRequired();
        builder.Property(i => i.MaxDepositAmount).HasColumnName("max_deposit_amount").IsRequired();
        builder.Property(i => i.MinWithdrawAmount).HasColumnName("min_withdraw_amount").IsRequired();
        builder.Property(i => i.MaxWithdrawAmount).HasColumnName("max_withdraw_amount").IsRequired();
        builder.Property(i => i.IsDepositActive).HasColumnName("is_deposit_active").IsRequired();
        builder.Property(i => i.IsWithdrawActive).HasColumnName("is_withdraw_active").IsRequired();
        builder.Property(i => i.DepositLimitExceeded).HasColumnName("deposit_limit_exceeded").IsRequired();
        builder.Property(i => i.WithdrawLimitExceeded).HasColumnName("withdraw_limit_exceeded").IsRequired();
        builder.Property(i => i.CommissionRate).HasColumnName("commission_rate").IsRequired();
        
        
        // Indexes
        builder.HasIndex(i => i.Name).IsUnique();
    }
}