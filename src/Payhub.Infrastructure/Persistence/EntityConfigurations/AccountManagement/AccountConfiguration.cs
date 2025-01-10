using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payhub.Domain.Entities.AccountManagement;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.AccountManagement;

public class AccountConfiguration : BaseEntityConfiguration<Account>
{
    public override void Configure(EntityTypeBuilder<Account> builder)
    {
        base.Configure(builder); 
        builder.ToTable("accounts");
        
        builder.Property(i => i.Name).HasColumnName("name").IsRequired();
        builder.Property(i => i.AccountNumber).HasColumnName("account_number").IsRequired();
        builder.Property(i => i.IsActive).HasColumnName("is_active").IsRequired();
        builder.Property(i => i.FirstBalance).HasColumnName("first_balance").IsRequired();
        
        builder.Property(i => i.Password).HasColumnName("password");
        builder.Property(i => i.PhoneNumber).HasColumnName("phone_number");
        builder.Property(i => i.Email).HasColumnName("email");
        builder.Property(i => i.EmailPassword).HasColumnName("email_password");
        builder.Property(i => i.EmailImapPassword).HasColumnName("email_imap_password");
        
        builder.Property(i => i.MinDepositAmount).HasColumnName("min_deposit_amount").IsRequired();
        builder.Property(i => i.MaxDepositAmount).HasColumnName("max_deposit_amount").IsRequired();
        builder.Property(i => i.DailyDepositAmountLimit).HasColumnName("daily_deposit_amount_limit").IsRequired();
        builder.Property(i => i.DailyWithdrawAmountLimit).HasColumnName("daily_withdraw_amount_limit").IsRequired();
        
        builder.Property(i => i.PaymentWayId).HasColumnName("payment_way_id").IsRequired();
        builder.Property(i => i.AffiliateId).HasColumnName("affiliate_id");
        builder.Property(i => i.BankId).HasColumnName("bank_id").IsRequired();
        builder.Property(i => i.AccountType).HasColumnName("account_type").IsRequired();
        builder.Property(i => i.AccountClassification).HasColumnName("account_classification").IsRequired();
        builder.Property(i => i.IsDeleted).HasColumnName("is_deleted").IsRequired();
        
        
        // Indexes
        builder.HasIndex(i => i.AccountNumber).IsUnique();
    }
}