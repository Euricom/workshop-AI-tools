using System;

namespace MyApp.Core.ValueObjects;

public record Money
{
    public decimal Amount { get; }

    private Money(decimal amount)
    {
        Amount = amount;
    }

    public static Money Create(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Money amount cannot be negative", nameof(amount));
            
        return new Money(decimal.Round(amount, 2, MidpointRounding.AwayFromZero));
    }

    public static Money Zero => new(0);

    public static implicit operator decimal(Money money) => money.Amount;
    public static explicit operator Money(decimal amount) => Create(amount);

    public Money Add(Money other) => Create(Amount + other.Amount);
    public Money Subtract(Money other) => Create(Amount - other.Amount);
    public Money Multiply(decimal factor) => Create(Amount * factor);
}