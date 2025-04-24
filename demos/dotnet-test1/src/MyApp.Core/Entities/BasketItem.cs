namespace MyApp.Core.Entities;

public class BasketItem
{
    public string Id { get; private set; } = default!;
    public string ProductId { get; private set; } = default!;
    public int Quantity { get; private set; }

    private BasketItem() { } // For EF Core

    public static BasketItem Create(string productId, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        return new BasketItem
        {
            Id = Guid.NewGuid().ToString(),
            ProductId = productId,
            Quantity = quantity
        };
    }

    internal void UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        Quantity = quantity;
    }
}