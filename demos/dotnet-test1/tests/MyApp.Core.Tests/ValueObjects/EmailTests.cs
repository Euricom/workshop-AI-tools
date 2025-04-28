using System;
using Xunit;
using MyApp.Core.ValueObjects;

namespace MyApp.Core.Tests.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user.name@domain.com")]
    [InlineData("user+tag@example.co.uk")]
    public void Create_WithValidEmail_ShouldCreateEmail(string validEmail)
    {
        // Act
        var email = Email.Create(validEmail);

        // Assert
        Assert.NotNull(email);
        Assert.Equal(validEmail, email.Value);
    }

    [Theory]
    [InlineData(null, "Email cannot be empty (Parameter 'email')")]
    [InlineData("", "Email cannot be empty (Parameter 'email')")]
    [InlineData(" ", "Email cannot be empty (Parameter 'email')")]
    [InlineData("invalid", "Invalid email format (Parameter 'email')")]
    [InlineData("invalid@", "Invalid email format (Parameter 'email')")]
    [InlineData("@domain.com", "Invalid email format (Parameter 'email')")]
    [InlineData("user@domain", "Invalid email format (Parameter 'email')")]
    [InlineData("user@.com", "Invalid email format (Parameter 'email')")]
    [InlineData("user@domain.", "Invalid email format (Parameter 'email')")]
    public void Create_WithInvalidEmail_ShouldThrowArgumentException(string invalidEmail, string expectedMessage)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Email.Create(invalidEmail));
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void Create_WithWhitespace_ShouldTrimAndValidate()
    {
        // Arrange
        var emailWithWhitespace = " test@example.com ";
        var expectedEmail = "test@example.com";

        // Act
        var email = Email.Create(emailWithWhitespace);

        // Assert
        Assert.Equal(expectedEmail, email.Value);
    }

    [Fact]
    public void ImplicitConversion_ToStringValue_ShouldReturnEmailValue()
    {
        // Arrange
        var emailValue = "test@example.com";
        var email = Email.Create(emailValue);

        // Act
        string result = email;

        // Assert
        Assert.Equal(emailValue, result);
    }

    [Fact]
    public void ExplicitConversion_FromStringValue_ShouldCreateEmail()
    {
        // Arrange
        var emailValue = "test@example.com";

        // Act
        var email = (Email)emailValue;

        // Assert
        Assert.Equal(emailValue, email.Value);
    }

    [Fact]
    public void ToString_ShouldReturnEmailValue()
    {
        // Arrange
        var emailValue = "test@example.com";
        var email = Email.Create(emailValue);

        // Act
        var result = email.ToString();

        // Assert
        Assert.Equal(emailValue, result);
    }
}