using System;
using Xunit;
using MyApp.Core.Entities;
using MyApp.Core.ValueObjects;

namespace MyApp.Core.Tests.Entities;

public class ProductTests
{
    [Fact]
    public void Constructor_WithValidInputs_ShouldCreateProduct()
    {
        // Arrange
        var name = "Test Product";
        var description = "Test Description";
        var price = Money.Create(10.99m);
        var stockQuantity = 5;
        var reorderPoint = 2;

        // Act
        var product = new Product(name, description, price, stockQuantity, reorderPoint);

        // Assert
        Assert.NotEqual(Guid.Empty, product.Id);
        Assert.Equal(name, product.Name);
        Assert.Equal(description, product.Description);
        Assert.Equal(price, product.Price);
        Assert.Equal(stockQuantity, product.StockQuantity);
        Assert.Equal(reorderPoint, product.ReorderPoint);
        Assert.True(product.IsAvailable);
    }

    [Theory]
    [InlineData("", "Description", "Product name cannot be empty (Parameter 'name')")]
    [InlineData(" ", "Description", "Product name cannot be empty (Parameter 'name')")]
    [InlineData(null, "Description", "Product name cannot be empty (Parameter 'name')")]
    [InlineData("Name", "", "Product description cannot be empty (Parameter 'description')")]
    [InlineData("Name", " ", "Product description cannot be empty (Parameter 'description')")]
    [InlineData("Name", null, "Product description cannot be empty (Parameter 'description')")]
    public void Constructor_WithInvalidInputs_ShouldThrowArgumentException(string name, string description, string expectedMessage)
    {
        // Arrange
        var price = Money.Create(10.99m);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Product(name, description, price));
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_WithNegativeStockQuantity_ShouldThrowArgumentException(int stockQuantity)
    {
        // Arrange
        var name = "Test Product";
        var description = "Test Description";
        var price = Money.Create(10.99m);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Product(name, description, price, stockQuantity));
        Assert.Equal("Stock quantity cannot be negative (Parameter 'stockQuantity')", exception.Message);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_WithNegativeReorderPoint_ShouldThrowArgumentException(int reorderPoint)
    {
        // Arrange
        var name = "Test Product";
        var description = "Test Description";
        var price = Money.Create(10.99m);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Product(name, description, price, 0, reorderPoint));
        Assert.Equal("Reorder point cannot be negative (Parameter 'reorderPoint')", exception.Message);
    }

    [Fact]
    public void UpdateDetails_WithValidInputs_ShouldUpdateProduct()
    {
        // Arrange
        var product = new Product("Old Name", "Old Description", Money.Create(10.99m));
        var newName = "New Name";
        var newDescription = "New Description";
        var newPrice = Money.Create(20.99m);

        // Act
        product.UpdateDetails(newName, newDescription, newPrice);

        // Assert
        Assert.Equal(newName, product.Name);
        Assert.Equal(newDescription, product.Description);
        Assert.Equal(newPrice, product.Price);
    }

    [Fact]
    public void SetAvailability_WhenOutOfStock_ShouldThrowException()
    {
        // Arrange
        var product = new Product("Test Product", "Test Description", Money.Create(10.99m), 0);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => product.SetAvailability(true));
        Assert.Equal("Cannot set product as available when out of stock", exception.Message);
    }

    [Fact]
    public void AddStock_ShouldIncreaseQuantityAndUpdateAvailability()
    {
        // Arrange
        var product = new Product("Test Product", "Test Description", Money.Create(10.99m), 0);
        Assert.False(product.IsAvailable);

        // Act
        product.AddStock(5);

        // Assert
        Assert.Equal(5, product.StockQuantity);
        Assert.True(product.IsAvailable);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AddStock_WithInvalidQuantity_ShouldThrowException(int quantity)
    {
        // Arrange
        var product = new Product("Test Product", "Test Description", Money.Create(10.99m));

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => product.AddStock(quantity));
        Assert.Equal("Quantity must be greater than zero (Parameter 'quantity')", exception.Message);
    }

    [Fact]
    public void RemoveStock_ShouldDecreaseQuantityAndUpdateAvailability()
    {
        // Arrange
        var product = new Product("Test Product", "Test Description", Money.Create(10.99m), 5);
        Assert.True(product.IsAvailable);

        // Act
        product.RemoveStock(5);

        // Assert
        Assert.Equal(0, product.StockQuantity);
        Assert.False(product.IsAvailable);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void RemoveStock_WithInvalidQuantity_ShouldThrowException(int quantity)
    {
        // Arrange
        var product = new Product("Test Product", "Test Description", Money.Create(10.99m), 5);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => product.RemoveStock(quantity));
        Assert.Equal("Quantity must be greater than zero (Parameter 'quantity')", exception.Message);
    }

    [Fact]
    public void RemoveStock_WhenInsufficientStock_ShouldThrowException()
    {
        // Arrange
        var product = new Product("Test Product", "Test Description", Money.Create(10.99m), 5);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => product.RemoveStock(6));
        Assert.Equal("Insufficient stock available", exception.Message);
    }

    [Fact]
    public void UpdateReorderPoint_WithValidValue_ShouldUpdateReorderPoint()
    {
        // Arrange
        var product = new Product("Test Product", "Test Description", Money.Create(10.99m));
        var newReorderPoint = 10;

        // Act
        product.UpdateReorderPoint(newReorderPoint);

        // Assert
        Assert.Equal(newReorderPoint, product.ReorderPoint);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void UpdateReorderPoint_WithNegativeValue_ShouldThrowException(int reorderPoint)
    {
        // Arrange
        var product = new Product("Test Product", "Test Description", Money.Create(10.99m));

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => product.UpdateReorderPoint(reorderPoint));
        Assert.Equal("Reorder point cannot be negative (Parameter 'reorderPoint')", exception.Message);
    }

    [Theory]
    [InlineData(5, 10, true)]  // Stock below reorder point
    [InlineData(10, 10, true)] // Stock at reorder point
    [InlineData(15, 10, false)] // Stock above reorder point
    public void NeedsReorder_ShouldReturnCorrectValue(int stockQuantity, int reorderPoint, bool expectedResult)
    {
        // Arrange
        var product = new Product("Test Product", "Test Description", Money.Create(10.99m), stockQuantity, reorderPoint);

        // Act
        var result = product.NeedsReorder();

        // Assert
        Assert.Equal(expectedResult, result);
    }
}