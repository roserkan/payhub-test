using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payhub.Domain.Entities.BotManagement;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.BotManagement;

public class WithdrawOrderConfiguration : BaseEntityConfiguration<WithdrawOrder>
{
    public override void Configure(EntityTypeBuilder<WithdrawOrder> builder)
    {
        base.Configure(builder); 
        builder.ToTable("withdraw_orders");
        
        builder.Property(i => i.AccountId).HasColumnName("account_id");
        builder.Property(i => i.ReceiverAccountNumber).HasColumnName("receiver_account_number").IsRequired();
        builder.Property(i => i.ReceiverFullName).HasColumnName("receiver_full_name").IsRequired();
        builder.Property(i => i.Amount).HasColumnName("amount").HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(i => i.Status).HasColumnName("status").IsRequired();
        builder.Property(i => i.Description).HasColumnName("description");
        builder.Property(i => i.TransactionDate).HasColumnType("timestamp without time zone").HasColumnName("transaction_date").IsRequired();
    }
}