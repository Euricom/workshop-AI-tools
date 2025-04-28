using MyApp.Core.ValueObjects;

namespace MyApp.Core.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Money Price { get; private set; }
    public bool IsAvailable { get; private set; }
    public int StockQuantity { get; private set; }
    public int ReorderPoint { get; private set; }

    private Product() { } // For EF Core

    public Product(string name, string description, Money price, int stockQuantity = 0, int reorderPoint = 5)
    {
        ValidateName(name);
        ValidateDescription(description);
        ValidateStockQuantity(stockQuantity);
        ValidateReorderPoint(reorderPoint);

        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
        ReorderPoint = reorderPoint;
        IsAvailable = stockQuantity > 0;
    }

    public void UpdateDetails(string name, string description, Money price)
    {
        ValidateName(name);
        ValidateDescription(description);

        Name = name;
        Description = description;
        Price = price;
    }

    public void SetAvailability(bool isAvailable)
    {
        if (isAvailable && StockQuantity <= 0)
            throw new InvalidOperationException("Cannot set product as available when out of stock");

        IsAvailable = isAvailable;
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        StockQuantity += quantity;
        if (StockQuantity > 0 && !IsAvailable)
            IsAvailable = true;
    }

    public void RemoveStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        if (quantity > StockQuantity)
            throw new InvalidOperationException("Insufficient stock available");

        StockQuantity -= quantity;
        if (StockQuantity == 0)
            IsAvailable = false;
    }

    public void UpdateReorderPoint(int reorderPoint)
    {
        ValidateReorderPoint(reorderPoint);
        ReorderPoint = reorderPoint;
    }

    public bool NeedsReorder() => StockQuantity <= ReorderPoint;

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty", nameof(name));
        if (name.Length > 200)
            throw new ArgumentException("Product name cannot exceed 200 characters", nameof(name));
    }

    private static void ValidateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Product description cannot be empty", nameof(description));
        if (description.Length > 2000)
            throw new ArgumentException("Product description cannot exceed 2000 characters", nameof(description));
    }

    private static void ValidateStockQuantity(int stockQuantity)
    {
        if (stockQuantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative", nameof(stockQuantity));
    }

    private static void ValidateReorderPoint(int reorderPoint)
    {
        if (reorderPoint < 0)
            throw new ArgumentException("Reorder point cannot be negative", nameof(reorderPoint));
    }
}