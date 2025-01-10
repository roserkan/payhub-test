using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain;

namespace Payhub.Infrastructure.Persistence.EntityConfigurations;

public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);
        
        // PostgreSQL'de Id sütunu otomatik artan identity olarak yapılandırılıyor
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()  // PostgreSQL'de otomatik artış için
            .IsRequired();
        
        // CreatedDate ve UpdatedDate sütunlarının Türkiye saatine göre kaydedilmesini sağlıyoruz
        builder.Property(e => e.CreatedDate)
            .HasColumnName("created_date")
            .HasColumnType("timestamp without time zone")  // Zaman dilimsiz timestamp kullanılıyor
            .IsRequired();

        builder.Property(e => e.UpdatedDate)
            .HasColumnName("updated_date")
            .HasColumnType("timestamp without time zone")
            .IsRequired(false);
    }
}