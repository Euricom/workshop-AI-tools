using MyApp.Core.ValueObjects;

namespace MyApp.Core.Entities;

public class Customer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    private readonly List<Order> _orders;
    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

    private Customer() 
    {
        _orders = new List<Order>();
    }

    public Customer(string name, Email email, PhoneNumber phoneNumber)
    {
        ValidateName(name);

        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        _orders = new List<Order>();
    }

    public void UpdateContactInfo(Email email, PhoneNumber phoneNumber)
    {
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public void UpdateName(string name)
    {
        ValidateName(name);
        Name = name;
    }

    public Order CreateOrder()
    {
        var order = new Order(this);
        _orders.Add(order);
        return order;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name cannot be empty", nameof(name));
        if (name.Length > 100)
            throw new ArgumentException("Customer name cannot exceed 100 characters", nameof(name));
    }
}