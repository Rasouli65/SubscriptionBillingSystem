using SubscriptionBillingSystem.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace SubscriptionBillingSystem.Domain.ValueObjects
{
    public record Email
    {
        public string Value { get; init; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Email cannot be empty.");

            if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new DomainException("Invalid email format.");

            Value = value;
        }
    }
}
