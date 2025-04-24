namespace MyApp.Core.Entities;

public class Product
{
    public string Id { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public decimal Price { get; private set; }
    public bool Available { get; private set; }
    public string Category { get; private set; } = default!;

    private Product() { } // For EF Core

    public static Product Create(string name, string description, decimal price, bool available, string category)
    {
        if (price < 0)
            throw new ArgumentException("Price cannot be negative", nameof(price));

        return new Product
        {
            Id = Guid.NewGuid().ToString(),
            Name = name,
            Description = description,
            Price = price,
            Available = available,
            Category = category
        };
    }

    public void Update(string? name = null, string? description = null, decimal? price = null, bool? available = null, string? category = null)
    {
        if (name != null) Name = name;
        if (description != null) Description = description;
        if (price.HasValue)
        {
            if (price.Value < 0)
                throw new ArgumentException("Price cannot be negative", nameof(price));
            Price = price.Value;
        }
        if (available.HasValue) Available = available.Value;
        if (category != null) Category = category;
    }
}