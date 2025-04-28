using System;
using Xunit;
using MyApp.Core.Entities;
using MyApp.Core.ValueObjects;

namespace MyApp.Core.Tests.Entities;

public class CustomerTests
{
    private static readonly Email ValidEmail = Email.Create("test@example.com");
    private static readonly PhoneNumber ValidPhone = PhoneNumber.Create("+1234567890");

    [Fact]
    public void Constructor_WithValidInputs_ShouldCreateCustomer()
    {
        // Arrange
        var name = "John Doe";
        var email = ValidEmail;
        var phoneNumber = ValidPhone;

        // Act
        var customer = new Customer(name, email, phoneNumber);

        // Assert
        Assert.NotEqual(Guid.Empty, customer.Id);
        Assert.Equal(name, customer.Name);
        Assert.Equal(email, customer.Email);
        Assert.Equal(phoneNumber, customer.PhoneNumber);
        Assert.Empty(customer.Orders);
    }

    [Theory]
    [InlineData("", "Customer name cannot be empty (Parameter 'name')")]
    [InlineData(" ", "Customer name cannot be empty (Parameter 'name')")]
    [InlineData(null, "Customer name cannot be empty (Parameter 'name')")]
    public void Constructor_WithInvalidName_ShouldThrowArgumentException(string name, string expectedMessage)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Customer(name, ValidEmail, ValidPhone));
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void Constructor_WithNameTooLong_ShouldThrowArgumentException()
    {
        // Arrange
        var longName = new string('x', 101); // 101 characters

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Customer(longName, ValidEmail, ValidPhone));
        Assert.Equal("Customer name cannot exceed 100 characters (Parameter 'name')", exception.Message);
    }

    [Fact]
    public void UpdateContactInfo_ShouldUpdateEmailAndPhone()
    {
        // Arrange
        var customer = new Customer("John Doe", ValidEmail, ValidPhone);
        var newEmail = Email.Create("new@example.com");
        var newPhone = PhoneNumber.Create("+9876543210");

        // Act
        customer.UpdateContactInfo(newEmail, newPhone);

        // Assert
        Assert.Equal(newEmail, customer.Email);
        Assert.Equal(newPhone, customer.PhoneNumber);
    }

    [Theory]
    [InlineData("", "Customer name cannot be empty (Parameter 'name')")]
    [InlineData(" ", "Customer name cannot be empty (Parameter 'name')")]
    [InlineData(null, "Customer name cannot be empty (Parameter 'name')")]
    public void UpdateName_WithInvalidName_ShouldThrowArgumentException(string name, string expectedMessage)
    {
        // Arrange
        var customer = new Customer("John Doe", ValidEmail, ValidPhone);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => customer.UpdateName(name));
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void UpdateName_WithValidName_ShouldUpdateName()
    {
        // Arrange
        var customer = new Customer("John Doe", ValidEmail, ValidPhone);
        var newName = "Jane Doe";

        // Act
        customer.UpdateName(newName);

        // Assert
        Assert.Equal(newName, customer.Name);
    }

    [Fact]
    public void CreateOrder_ShouldAddOrderToCollection()
    {
        // Arrange
        var customer = new Customer("John Doe", ValidEmail, ValidPhone);

        // Act
        var order = customer.CreateOrder();

        // Assert
        Assert.Single(customer.Orders);
        Assert.Contains(order, customer.Orders);
        Assert.Equal(customer, order.Customer);
    }

    [Fact]
    public void Orders_ShouldBeReadOnly()
    {
        // Arrange
        var customer = new Customer("John Doe", ValidEmail, ValidPhone);
        customer.CreateOrder(); // Create one order

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => 
            ((System.Collections.Generic.ICollection<Order>)customer.Orders).Add(new Order(customer)));
    }
}