using System;
using System.Text.RegularExpressions;

namespace MyApp.Core.ValueObjects;

public record PhoneNumber
{
    private static readonly Regex PhoneRegex = new(
        @"^\+?[1-9]\d{7,14}$", // Minimum 8 digits total (1 + 7), maximum 15 digits
        RegexOptions.Compiled
    );

    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static PhoneNumber Create(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number cannot be empty", nameof(phoneNumber));

        // Remove any spaces, dashes, or parentheses
        var cleaned = Regex.Replace(phoneNumber.Trim(), @"[\s\-\(\)]", "");

        if (!PhoneRegex.IsMatch(cleaned))
            throw new ArgumentException("Invalid phone number format. Must be E.164 format.", nameof(phoneNumber));

        return new PhoneNumber(cleaned);
    }

    public static implicit operator string(PhoneNumber phone) => phone.Value;
    public static explicit operator PhoneNumber(string phone) => Create(phone);

    public override string ToString() => Value;
}