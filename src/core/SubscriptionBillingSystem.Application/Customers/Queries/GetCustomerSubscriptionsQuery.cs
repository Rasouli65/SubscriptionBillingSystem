using MediatR;

namespace SubscriptionBillingSystem.Application.Customers.Queries
{
    public record GetCustomerSubscriptionsQuery(Guid CustomerId) : IRequest<List<SubscriptionDto>>;

    public record SubscriptionDto(
        Guid Id,
        string Status,
        decimal PriceAmount,
        List<InvoiceDto> Invoices);

    public record InvoiceDto(Guid Id, decimal Amount, string Status);
}
