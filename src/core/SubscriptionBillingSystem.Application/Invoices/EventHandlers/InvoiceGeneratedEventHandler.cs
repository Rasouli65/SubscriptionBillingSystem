using MediatR;
using Microsoft.Extensions.Logging;
using SubscriptionBillingSystem.Domain.Aggregates.Invoice;

namespace SubscriptionBillingSystem.Application.Invoices.EventHandlers
{
    public class InvoiceGeneratedEventHandler : INotificationHandler<InvoiceGeneratedDomainEvent>
    {
        private readonly ILogger<InvoiceGeneratedEventHandler> _logger;

        public InvoiceGeneratedEventHandler(ILogger<InvoiceGeneratedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(InvoiceGeneratedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Invoice {InvoiceId} has been generated for amount {Amount}",
                notification.InvoiceId, notification.Amount);

            return Task.CompletedTask;
        }
    }
}
