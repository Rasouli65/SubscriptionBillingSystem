using MediatR;

namespace SubscriptionBillingSystem.Domain.Aggregates.Invoice;

public record PaymentReceivedDomainEvent(Guid InvoiceId, DateTime PaidAt) : INotification;
