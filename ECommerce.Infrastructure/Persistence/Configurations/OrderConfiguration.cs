using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.OrderId);

            builder.Property(o => o.Amount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(o => o.Currency)
                .IsRequired()
                .HasMaxLength(3);

            builder.Property(o => o.ProductIds)
                .IsRequired()
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
                );

            builder.Property(o => o.Status)
                .IsRequired();

            builder.Property(o => o.CreatedAt)
                .IsRequired();
        }
    }
} 