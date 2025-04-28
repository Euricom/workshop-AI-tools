using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Core.Entities;

namespace MyApp.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.OwnsOne(p => p.Price, priceBuilder =>
        {
            priceBuilder.Property(m => m.Amount)
                .HasColumnName("Price")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        });

        builder.Property(p => p.IsAvailable)
            .IsRequired();

        builder.Property(p => p.StockQuantity)
            .IsRequired();

        builder.Property(p => p.ReorderPoint)
            .IsRequired();
    }
}