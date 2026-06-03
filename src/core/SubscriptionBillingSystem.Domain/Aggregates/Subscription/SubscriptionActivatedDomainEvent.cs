using MediatR;
using SubscriptionBillingSystem.Domain.ValueObjects;

namespace SubscriptionBillingSystem.Domain.Aggregates.Subscription
{
    public record SubscriptionActivatedDomainEvent(
        Guid SubscriptionId,
        Guid CustomerId,
        Money Price) : INotification;
}
