using SubscriptionBillingSystem.Domain.Common;
using SubscriptionBillingSystem.Domain.ValueObjects;

namespace SubscriptionBillingSystem.Domain.Aggregates.Invoice
{
    public class Invoice : Entity, IAggregateRoot
    {
        public Guid SubscriptionId { get; private set; }
        public Guid CustomerId { get; private set; }
        public Money Amount { get; private set; }
        public InvoiceStatus Status { get; private set; }
        public DateTime IssuedAt { get; private set; }
        public DateTime? PaidAt { get; private set; }

        private Invoice() { }

        public Invoice(Guid subscriptionId, Guid customerId, Money amount)
        {
            SubscriptionId = subscriptionId;
            CustomerId = customerId;
            Amount = amount;
            Status = InvoiceStatus.Pending;
            IssuedAt = DateTime.UtcNow;

            // Publish invoice generated domain event
            AddDomainEvent(new InvoiceGeneratedDomainEvent(Id, SubscriptionId, Amount));
        }

        public void Pay()
        {
            // The invoice should not be paid twice
            if (Status == InvoiceStatus.Paid)
            {
                throw new InvalidOperationException("This invoice has already been paid.");
            }

            if (Status == InvoiceStatus.Cancelled)
            {
                throw new InvalidOperationException("A cancelled invoice cannot be paid.");
            }

            Status = InvoiceStatus.Paid;
            PaidAt = DateTime.UtcNow;

            // Publish payment received domain event
            AddDomainEvent(new PaymentReceivedDomainEvent(Id, DateTime.UtcNow));
        }
    }
}
