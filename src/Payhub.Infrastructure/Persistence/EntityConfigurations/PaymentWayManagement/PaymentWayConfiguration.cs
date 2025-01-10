using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payhub.Domain.Entities.PaymentWayManagement;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.PaymentWayManagement;

public class PaymentWayConfiguration : BaseEntityConfiguration<PaymentWay>
{
    public override void Configure(EntityTypeBuilder<PaymentWay> builder)
    {
        base.Configure(builder); 
        builder.ToTable("payment_ways");
        
        builder.Property(i => i.Name).HasColumnName("name").IsRequired();
        
        
        // Indexes
        builder.HasIndex(i => i.Name).IsUnique();
    }
}