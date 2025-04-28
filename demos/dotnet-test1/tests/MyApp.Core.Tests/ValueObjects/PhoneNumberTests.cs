using System;
using Xunit;
using MyApp.Core.ValueObjects;

namespace MyApp.Core.Tests.ValueObjects;

public class PhoneNumberTests
{
    [Theory]
    [InlineData("+1234567890")]
    [InlineData("+442071234567")]
    [InlineData("+32123456789")]
    public void Create_WithValidPhoneNumber_ShouldCreatePhoneNumber(string validNumber)
    {
        // Act
        var phoneNumber = PhoneNumber.Create(validNumber);

        // Assert
        Assert.NotNull(phoneNumber);
        Assert.Equal(validNumber, phoneNumber.Value);
    }

    [Theory]
    [InlineData(null, "Phone number cannot be empty (Parameter 'phoneNumber')")]
    [InlineData("", "Phone number cannot be empty (Parameter 'phoneNumber')")]
    [InlineData(" ", "Phone number cannot be empty (Parameter 'phoneNumber')")]
    [InlineData("invalid", "Invalid phone number format. Must be E.164 format. (Parameter 'phoneNumber')")]
    [InlineData("123", "Invalid phone number format. Must be E.164 format. (Parameter 'phoneNumber')")] // Too short
    [InlineData("+0123456789", "Invalid phone number format. Must be E.164 format. (Parameter 'phoneNumber')")] // Starting with +0
    [InlineData("+", "Invalid phone number format. Must be E.164 format. (Parameter 'phoneNumber')")]
    public void Create_WithInvalidPhoneNumber_ShouldThrowArgumentException(string invalidNumber, string expectedMessage)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => PhoneNumber.Create(invalidNumber));
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Theory]
    [InlineData(" +1234567890 ", "+1234567890")]
    [InlineData("+44 207 123 4567", "+442071234567")]
    [InlineData("+32 (123) 456-789", "+32123456789")]
    public void Create_WithFormattedNumber_ShouldStripFormattingAndCreate(string input, string expected)
    {
        // Act
        var phoneNumber = PhoneNumber.Create(input);

        // Assert
        Assert.Equal(expected, phoneNumber.Value);
    }

    [Theory]
    [InlineData("+1234567890123456")] // Too long (>15 digits)
    [InlineData("+1")] // Too short (<3 digits)
    public void Create_WithInvalidLength_ShouldThrowArgumentException(string invalidNumber)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => PhoneNumber.Create(invalidNumber));
    }

    [Fact]
    public void ImplicitConversion_ToStringValue_ShouldReturnPhoneNumberValue()
    {
        // Arrange
        var numberValue = "+1234567890";
        var phoneNumber = PhoneNumber.Create(numberValue);

        // Act
        string result = phoneNumber;

        // Assert
        Assert.Equal(numberValue, result);
    }

    [Fact]
    public void ExplicitConversion_FromStringValue_ShouldCreatePhoneNumber()
    {
        // Arrange
        var numberValue = "+1234567890";

        // Act
        var phoneNumber = (PhoneNumber)numberValue;

        // Assert
        Assert.Equal(numberValue, phoneNumber.Value);
    }

    [Fact]
    public void ToString_ShouldReturnPhoneNumberValue()
    {
        // Arrange
        var numberValue = "+1234567890";
        var phoneNumber = PhoneNumber.Create(numberValue);

        // Act
        var result = phoneNumber.ToString();

        // Assert
        Assert.Equal(numberValue, result);
    }
}