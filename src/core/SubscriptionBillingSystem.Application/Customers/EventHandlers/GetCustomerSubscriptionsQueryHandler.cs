using MediatR;
using Microsoft.EntityFrameworkCore;
using SubscriptionBillingSystem.Application.Abstractions;
using SubscriptionBillingSystem.Application.Customers.Queries;

namespace SubscriptionBillingSystem.Application.Customers.EventHandlers
{
    public class GetCustomerSubscriptionsQueryHandler
    : IRequestHandler<GetCustomerSubscriptionsQuery, List<SubscriptionDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetCustomerSubscriptionsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SubscriptionDto>> Handle(GetCustomerSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Subscriptions
                .Where(s => s.CustomerId == request.CustomerId)
                .Select(s => new SubscriptionDto(
                    s.Id,
                    s.Status.ToString(),
                    s.Price.Amount,
                    s.Invoices.Select(i => new InvoiceDto(i.Id, i.Amount.Amount, i.Status.ToString())).ToList()
                ))
                .ToListAsync(cancellationToken);
        }
    }
}
