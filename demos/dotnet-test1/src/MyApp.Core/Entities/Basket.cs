namespace MyApp.Core.Entities;

public class Basket
{
    private readonly List<BasketItem> _items = new();
    
    public string Id { get; private set; } = default!;
    public string UserId { get; private set; } = default!;
    public IReadOnlyCollection<BasketItem> Items => _items.AsReadOnly();
    public decimal Total { get; private set; }

    private Basket() { } // For EF Core

    public static Basket CreateForUser(string userId)
    {
        return new Basket
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userId,
            Total = 0
        };
    }

    public void AddItem(string productId, int quantity, decimal productPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            _items.Add(BasketItem.Create(productId, quantity));
        }

        RecalculateTotal(productPrice);
    }

    public void RemoveItem(string productId, decimal productPrice)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            _items.Remove(item);
            RecalculateTotal(productPrice);
        }
    }

    public void UpdateItemQuantity(string productId, int quantity, decimal productPrice)
    {
        if (quantity <= 0)
        {
            RemoveItem(productId, productPrice);
            return;
        }

        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            item.UpdateQuantity(quantity);
            RecalculateTotal(productPrice);
        }
    }

    private void RecalculateTotal(decimal productPrice)
    {
        Total = _items.Sum(item => item.Quantity * productPrice);
    }
}