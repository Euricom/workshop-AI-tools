using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Core.Entities;

namespace MyApp.Infrastructure.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.OrderId)
            .IsRequired();

        builder.Property(i => i.ProductId)
            .IsRequired();

        builder.Property(i => i.Quantity)
            .IsRequired();

        builder.OwnsOne(i => i.UnitPrice, priceBuilder =>
        {
            priceBuilder.Property(m => m.Amount)
                .HasColumnName("UnitPrice")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        });

        // Configure many-to-one relationship with Product
        builder.HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure many-to-one relationship with Order
        builder.HasOne(i => i.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ignore computed property
        builder.Ignore(i => i.SubTotal);
    }
}