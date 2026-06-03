using SubscriptionBillingSystem.Domain.Exceptions;

namespace SubscriptionBillingSystem.Domain.ValueObjects
{
    public record Money
    {
        public decimal Amount { get; init; }
        public string Currency { get; init; }

        private Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public static Money Create(decimal amount, string currency = "USD")
        {
            if (amount < 0)
                throw new DomainException("Amount cannot be negative.");

            if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3)
                throw new DomainException("Currency must be a 3-letter ISO code (e.g., USD).");

            return new Money(amount, currency.ToUpper());
        }

        public override string ToString() => $"{Amount} {Currency}";
    }
}
