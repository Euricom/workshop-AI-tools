namespace MyApp.Infrastructure.Persistence.Configurations;

public class BasketConfiguration : IEntityTypeConfiguration<Basket>
{
    public void Configure(EntityTypeBuilder<Basket> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.UserId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(b => b.Total)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasMany(b => b.Items)
            .WithOne()
            .HasForeignKey("BasketId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(b => b.UserId)
            .IsUnique();

        builder.Navigation(b => b.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
{
    public void Configure(EntityTypeBuilder<BasketItem> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.ProductId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(i => i.Quantity)
            .IsRequired();

        builder.HasIndex("BasketId", nameof(BasketItem.ProductId))
            .IsUnique();
    }
}