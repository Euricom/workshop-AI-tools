using MyApp.Core.ValueObjects;

namespace MyApp.Core.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Order Order { get; private set; }
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; }
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; }
    public Money SubTotal => UnitPrice.Multiply(Quantity);

    private OrderItem() { } // For EF Core

    public OrderItem(Order order, Product product, int quantity)
    {
        ValidateQuantity(quantity);
        
        if (order == null)
            throw new ArgumentNullException(nameof(order));
        
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        Id = Guid.NewGuid();
        Order = order;
        OrderId = order.Id;
        Product = product;
        ProductId = product.Id;
        UnitPrice = product.Price;
        Quantity = quantity;
    }

    public void UpdateQuantity(int newQuantity)
    {
        ValidateQuantity(newQuantity);
        Quantity = newQuantity;
    }

    private static void ValidateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
    }
}