using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payhub.Domain.Entities.TransactionManagement;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.TransactionManagement;

public class DepositConfiguration : BaseEntityConfiguration<Deposit>
{
    public override void Configure(EntityTypeBuilder<Deposit> builder)
    {
        base.Configure(builder); 
        builder.ToTable("deposits");
        
        builder.Property(i => i.SiteId).HasColumnName("site_id").IsRequired(); // FK
        builder.Property(i => i.ProcessId).HasColumnName("process_id").IsRequired();
        builder.Property(i => i.AccountId).HasColumnName("account_id");
        builder.Property(i => i.PanelCustomerId).HasColumnName("panel_customer_id").IsRequired();
        builder.Property(i => i.SiteCustomerId).HasColumnName("site_customer_id").IsRequired();
        builder.Property(i => i.CustomerFullName).HasColumnName("customer_full_name");
        builder.Property(i => i.Amount).HasColumnName("amount").IsRequired();
        builder.Property(i => i.PayedAmount).HasColumnName("payed_amount").IsRequired();
        builder.Property(i => i.RedirectUrl).HasColumnName("redirect_url");
        builder.Property(i => i.PaymentWayId).HasColumnName("payment_way_id").IsRequired(); // FK
        builder.Property(i => i.Status).HasColumnName("status").IsRequired();
        builder.Property(i => i.InfraConfirmed).HasColumnName("infra_confirmed").IsRequired();
        builder.Property(i => i.PaymentId).HasColumnName("payment_id").IsRequired();
        builder.Property(i => i.Commission).HasColumnName("commission").IsRequired();
        builder.Property(i => i.TransactionDate).HasColumnName("transaction_date").HasColumnType("timestamp without time zone");
        builder.Property(i => i.DynamicAccountName).HasColumnName("dynamic_account_name");
        builder.Property(i => i.DynamicAccountNumber).HasColumnName("dynamic_account_number");
        builder.Property(i => i.CreatedUserId).HasColumnName("created_user_id");
        builder.Property(i => i.UpdatedUserId).HasColumnName("updated_user_id");
        builder.Property(i => i.AutoUpdatedName).HasColumnName("auto_updated_name");
        builder.Property(i => i.AffiliateId).HasColumnName("affiliate_id");
        builder.Property(i => i.InfraCallbackType).HasColumnName("infra_callback_type");
        builder.Property(i => i.AffiliateCommission).HasColumnName("affiliate_commission").IsRequired();

        // Indexes
        builder.HasIndex(i => i.ProcessId).IsUnique();
        //builder.HasIndex(i => i.CreatedDate);
        builder.HasIndex(i => new { i.Status, i.CreatedDate });
    }
}