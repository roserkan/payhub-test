using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payhub.Domain.Entities.CustomerManagement;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations.CustomerManagement;


public class CustomerConfiguration : BaseEntityConfiguration<Customer>
{
    public override void Configure(EntityTypeBuilder<Customer> builder)
    {
        base.Configure(builder); 
        builder.ToTable("customers");
        
        builder.Property(i => i.SiteId).HasColumnName("site_id").IsRequired();
        builder.Property(i => i.SiteCustomerId).HasColumnName("site_customer_id");
        builder.Property(i => i.FullName).HasColumnName("full_name");
        builder.Property(i => i.Username).HasColumnName("username");
        builder.Property(i => i.SignupDate).HasColumnName("signup_date");
        builder.Property(i => i.IdentityNumber).HasColumnName("identity_number");
        builder.Property(i => i.CustomerIpAddress).HasColumnName("customer_ip_address");
        builder.Property(i => i.PanelCustomerId).HasColumnName("panel_customer_id").IsRequired();
        
        // Indexes
        builder.HasIndex(i => i.PanelCustomerId).IsUnique();
    }
}