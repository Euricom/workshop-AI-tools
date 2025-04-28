using MyApp.Core.ValueObjects;

namespace MyApp.Core.Entities;

public class Order
{
    private readonly List<OrderItem> _items;

    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public Money TotalAmount => Money.Create(_items.Sum(item => item.SubTotal.Amount));

    private Order() 
    {
        _items = new List<OrderItem>();
    }

    public Order(Customer customer)
    {
        if (customer == null)
            throw new ArgumentNullException(nameof(customer));

        Id = Guid.NewGuid();
        CustomerId = customer.Id;
        Customer = customer;
        Status = OrderStatus.Created;
        CreatedAt = DateTime.UtcNow;
        _items = new List<OrderItem>();
    }

    public void AddItem(Product product, int quantity)
    {
        if (Status != OrderStatus.Created)
            throw new InvalidOperationException("Cannot modify a confirmed or cancelled order");

        if (product == null)
            throw new ArgumentNullException(nameof(product));

        if (!product.IsAvailable)
            throw new InvalidOperationException("Cannot add unavailable product to order");

        var existingItem = _items.FirstOrDefault(i => i.ProductId == product.Id);
        if (existingItem != null)
        {
            var newQuantity = existingItem.Quantity + quantity;
            UpdateItemQuantity(product.Id, newQuantity);
        }
        else
        {
            _items.Add(new OrderItem(this, product, quantity));
            // Attempt to reserve the stock
            product.RemoveStock(quantity);
        }
    }

    public void RemoveItem(Guid productId)
    {
        if (Status != OrderStatus.Created)
            throw new InvalidOperationException("Cannot modify a confirmed or cancelled order");

        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            _items.Remove(item);
            // Return the stock
            var product = item.Product;
            product.AddStock(item.Quantity);
        }
    }

    public void UpdateItemQuantity(Guid productId, int newQuantity)
    {
        if (Status != OrderStatus.Created)
            throw new InvalidOperationException("Cannot modify a confirmed or cancelled order");

        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null)
            throw new InvalidOperationException("Item not found in order");

        var quantityDifference = newQuantity - item.Quantity;
        if (quantityDifference != 0)
        {
            if (quantityDifference > 0)
            {
                // Need to reserve more stock
                item.Product.RemoveStock(quantityDifference);
            }
            else
            {
                // Return some stock
                item.Product.AddStock(Math.Abs(quantityDifference));
            }
            item.UpdateQuantity(newQuantity);
        }
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Created)
            throw new InvalidOperationException("Order has already been confirmed or cancelled");

        if (!_items.Any())
            throw new InvalidOperationException("Cannot confirm an empty order");

        Status = OrderStatus.Confirmed;
        ConfirmedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Order is already cancelled");

        if (Status == OrderStatus.Confirmed)
            throw new InvalidOperationException("Cannot cancel a confirmed order");

        // Return all stock
        foreach (var item in _items)
        {
            item.Product.AddStock(item.Quantity);
        }

        Status = OrderStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
    }
}

public enum OrderStatus
{
    Created,
    Confirmed,
    Cancelled
}