using MediatR;

namespace SubscriptionBillingSystem.Application.Invoices.Commands.PayInvoice
{
    public record PayInvoiceCommand(Guid InvoiceId) : IRequest;
}
