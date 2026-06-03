using MediatR;
using SubscriptionBillingSystem.Application.Abstractions;
using SubscriptionBillingSystem.Domain.Aggregates.Invoice;
using SubscriptionBillingSystem.Domain.Aggregates.Subscription;
using SubscriptionBillingSystem.Domain.ValueObjects;

namespace SubscriptionBillingSystem.Application.Subscriptions.EventHandlers
{
    public class SubscriptionActivatedEventHandler : INotificationHandler<SubscriptionActivatedDomainEvent>
    {
        private readonly IApplicationDbContext _context;

        public SubscriptionActivatedEventHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(SubscriptionActivatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var price = Money.Create(notification.Price.Amount, notification.Price.Currency);
            var firstInvoice = new Invoice(
                notification.SubscriptionId,
                notification.CustomerId,
                price);

            await _context.Invoices.AddAsync(firstInvoice, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
