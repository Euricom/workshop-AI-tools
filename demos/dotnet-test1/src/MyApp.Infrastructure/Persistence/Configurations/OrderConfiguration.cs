using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Core.Entities;

namespace MyApp.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.CustomerId)
            .IsRequired();

        builder.Property(o => o.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(o => o.CreatedAt)
            .IsRequired();

        builder.Property(o => o.ConfirmedAt);
        builder.Property(o => o.CancelledAt);

        // Configure one-to-many relationship with OrderItems
        builder.HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure many-to-one relationship with Customer
        builder.HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Ignore computed property
        builder.Ignore(o => o.TotalAmount);
    }
}