namespace MyApp.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(p => p.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.Available)
            .IsRequired();

        builder.Property(p => p.Category)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(p => p.Category);
        
        builder.HasIndex(p => p.Name);
    }
}