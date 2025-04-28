using System;
using System.Text.RegularExpressions;

namespace MyApp.Core.ValueObjects;

public record Email
{
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        email = email.Trim();

        if (!EmailRegex.IsMatch(email))
            throw new ArgumentException("Invalid email format", nameof(email));

        return new Email(email);
    }

    public static implicit operator string(Email email) => email.Value;
    public static explicit operator Email(string email) => Create(email);

    public override string ToString() => Value;
}