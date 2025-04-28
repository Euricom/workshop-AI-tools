using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Core.Entities;
using MyApp.Core.ValueObjects;

namespace MyApp.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.OwnsOne(c => c.Email, emailBuilder =>
        {
            emailBuilder.Property(e => e.Value)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(320); // Maximum length of a valid email address
        });

        builder.OwnsOne(c => c.PhoneNumber, phoneBuilder =>
        {
            phoneBuilder.Property(p => p.Value)
                .HasColumnName("PhoneNumber")
                .IsRequired()
                .HasMaxLength(15); // E.164 format maximum length
        });

        // Configure one-to-many relationship with Orders
        builder.HasMany(c => c.Orders)
            .WithOne()
            .HasForeignKey("CustomerId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}