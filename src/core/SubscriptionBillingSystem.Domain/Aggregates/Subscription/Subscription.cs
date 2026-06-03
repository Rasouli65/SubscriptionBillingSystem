using SubscriptionBillingSystem.Domain.Common;
using SubscriptionBillingSystem.Domain.ValueObjects;

namespace SubscriptionBillingSystem.Domain.Aggregates.Subscription
{
    public class Subscription : Entity, IAggregateRoot
    {
        public Guid CustomerId { get; private set; }
        public SubscriptionStatus Status { get; private set; }
        public Money Price { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ActivatedAt { get; private set; }
        public DateTime? ExpirationDate { get; private set; }

        private readonly List<Invoice.Invoice> _invoices = new();

        public virtual IReadOnlyCollection<Invoice.Invoice> Invoices => _invoices.AsReadOnly();

        private Subscription() { }

        public Subscription(Guid customerId, Money price)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            Price = price;
            Status = SubscriptionStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            if (Status == SubscriptionStatus.Active) return;

            Status = SubscriptionStatus.Active;
            ActivatedAt = DateTime.UtcNow;

            ExpirationDate = ActivatedAt.Value.AddMinutes(1);

            AddDomainEvent(new SubscriptionActivatedDomainEvent(Id, CustomerId, Price));
        }

        public void Cancel()
        {
            if (Status == SubscriptionStatus.Cancelled) return;
            Status = SubscriptionStatus.Cancelled;
        }

        public void Expire()
        {
            if (Status == SubscriptionStatus.Expired) return;
            Status = SubscriptionStatus.Expired;
        }
    }
}
