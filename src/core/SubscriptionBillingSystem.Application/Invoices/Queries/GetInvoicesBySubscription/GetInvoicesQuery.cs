using MediatR;

namespace SubscriptionBillingSystem.Application.Invoices.Queries.GetInvoicesBySubscription
{
    public record GetInvoicesQuery(Guid SubscriptionId) : IRequest<List<InvoiceDto>>;

    public record InvoiceDto(
        Guid Id,
        decimal Amount,
        string Currency,
        string Status,
        DateTime IssuedAt,
        DateTime? PaidAt);
}
