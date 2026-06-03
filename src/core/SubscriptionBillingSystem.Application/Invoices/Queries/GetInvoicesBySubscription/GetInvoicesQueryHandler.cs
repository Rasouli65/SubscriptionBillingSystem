using MediatR;
using Microsoft.EntityFrameworkCore;
using SubscriptionBillingSystem.Application.Abstractions;

namespace SubscriptionBillingSystem.Application.Invoices.Queries.GetInvoicesBySubscription
{
    public class GetInvoicesQueryHandler : IRequestHandler<GetInvoicesQuery, List<InvoiceDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetInvoicesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<InvoiceDto>> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Invoices
                .AsNoTracking()
                .Where(i => i.SubscriptionId == request.SubscriptionId)
                .Select(i => new InvoiceDto(
                    i.Id,
                    i.Amount.Amount,
                    i.Amount.Currency,
                    i.Status.ToString(),
                    i.IssuedAt,
                    i.PaidAt))
                .ToListAsync(cancellationToken);
        }
    }
}
