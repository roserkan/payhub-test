using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payhub.Domain.Entities.BotManagement;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.BotManagement;

public class DeviceConfiguration : BaseEntityConfiguration<Device>
{
    public override void Configure(EntityTypeBuilder<Device> builder)
    {
        base.Configure(builder); 
        builder.ToTable("devices");
        
        builder.Property(i => i.Name).HasColumnName("name").IsRequired();
        builder.Property(i => i.Description).HasColumnName("description");
        builder.Property(i => i.SerialNumber).HasColumnName("serial_number").IsRequired();
        builder.Property(i => i.AccountId).HasColumnName("account_id");
        
        
        // Indexes
        builder.HasIndex(i => i.SerialNumber).IsUnique();
    }
}