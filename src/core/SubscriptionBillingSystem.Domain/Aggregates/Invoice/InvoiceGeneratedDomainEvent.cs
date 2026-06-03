using MediatR;
using SubscriptionBillingSystem.Domain.ValueObjects;

namespace SubscriptionBillingSystem.Domain.Aggregates.Invoice
{
    public record InvoiceGeneratedDomainEvent(Guid InvoiceId, Guid SubscriptionId, Money Amount) : INotification;
}
