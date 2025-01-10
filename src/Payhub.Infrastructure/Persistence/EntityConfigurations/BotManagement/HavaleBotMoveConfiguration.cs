using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payhub.Domain.Entities.BotManagement;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.BotManagement;

public class HavaleBotMoveConfiguration : BaseEntityConfiguration<HavaleBotMove>
{
    public override void Configure(EntityTypeBuilder<HavaleBotMove> builder)
    {
        base.Configure(builder); 
        builder.ToTable("havale_bot_moves");
        
        builder.Property(i => i.SenderName).HasColumnName("sender_name").IsRequired();
        builder.Property(i => i.Amount).HasColumnName("amount").IsRequired();
        builder.Property(i => i.ReceiverAccId).HasColumnName("receiver_acc_id").IsRequired();
        builder.Property(i => i.TransferReceivedDate).HasColumnName("transfer_received_date")
            .HasColumnType("timestamp without time zone").IsRequired();
        builder.Property(i => i.SecurityKey).HasColumnName("security_key").IsRequired();
        builder.Property(i => i.Status).HasColumnName("status").IsRequired();
        builder.Property(i => i.TryCount).HasColumnName("try_count").IsRequired();
    }
}