using System;
using Xunit;
using MyApp.Core.ValueObjects;

namespace MyApp.Core.Tests.ValueObjects;

public class MoneyTests
{
    [Theory]
    [InlineData(100)]
    [InlineData(0)]
    [InlineData(99.99)]
    public void Create_WithValidAmount_ShouldCreateMoney(decimal amount)
    {
        // Act
        var money = Money.Create(amount);

        // Assert
        Assert.NotNull(money);
        Assert.Equal(amount, money.Amount);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(-0.01)]
    public void Create_WithNegativeAmount_ShouldThrowArgumentException(decimal amount)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Money.Create(amount));
        Assert.Equal("Money amount cannot be negative (Parameter 'amount')", exception.Message);
    }

    [Theory]
    [InlineData(100.555, 100.56)] // Rounds up
    [InlineData(100.554, 100.55)] // Rounds down
    [InlineData(100.995, 101.00)] // Rounds up to whole number
    public void Create_ShouldRoundToTwoDecimalPlaces(decimal input, decimal expected)
    {
        // Act
        var money = Money.Create(input);

        // Assert
        Assert.Equal(expected, money.Amount);
    }

    [Fact]
    public void Zero_ShouldReturnMoneyWithZeroAmount()
    {
        // Act
        var money = Money.Zero;

        // Assert
        Assert.Equal(0m, money.Amount);
    }

    [Fact]
    public void ImplicitConversion_ToDecimalValue_ShouldReturnAmount()
    {
        // Arrange
        var money = Money.Create(100.50m);

        // Act
        decimal amount = money;

        // Assert
        Assert.Equal(100.50m, amount);
    }

    [Fact]
    public void ExplicitConversion_FromDecimalValue_ShouldCreateMoney()
    {
        // Arrange
        decimal amount = 100.50m;

        // Act
        var money = (Money)amount;

        // Assert
        Assert.Equal(amount, money.Amount);
    }

    [Fact]
    public void Add_TwoMoneyValues_ShouldReturnCorrectSum()
    {
        // Arrange
        var money1 = Money.Create(100.50m);
        var money2 = Money.Create(50.75m);

        // Act
        var result = money1.Add(money2);

        // Assert
        Assert.Equal(151.25m, result.Amount);
    }

    [Fact]
    public void Subtract_TwoMoneyValues_ShouldReturnCorrectDifference()
    {
        // Arrange
        var money1 = Money.Create(100.50m);
        var money2 = Money.Create(50.75m);

        // Act
        var result = money1.Subtract(money2);

        // Assert
        Assert.Equal(49.75m, result.Amount);
    }

    [Theory]
    [InlineData(100.00, 2, 200.00)]
    [InlineData(100.00, 0.5, 50.00)]
    [InlineData(10.50, 3, 31.50)]
    public void Multiply_ByFactor_ShouldReturnCorrectProduct(decimal amount, decimal factor, decimal expected)
    {
        // Arrange
        var money = Money.Create(amount);

        // Act
        var result = money.Multiply(factor);

        // Assert
        Assert.Equal(expected, result.Amount);
    }

    [Fact]
    public void Subtract_ResultingInNegativeAmount_ShouldThrowArgumentException()
    {
        // Arrange
        var money1 = Money.Create(50.00m);
        var money2 = Money.Create(100.00m);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => money1.Subtract(money2));
    }
}