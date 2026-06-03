using MediatR;
using Microsoft.EntityFrameworkCore;
using SubscriptionBillingSystem.Application.Abstractions;

namespace SubscriptionBillingSystem.Application.Invoices.Commands.PayInvoice
{
    public class PayInvoiceCommandHandler : IRequestHandler<PayInvoiceCommand>
    {
        private readonly IApplicationDbContext _context;

        public PayInvoiceCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(PayInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.Id == request.InvoiceId, cancellationToken);

            if (invoice == null)
            {
                throw new KeyNotFoundException($"Invoice with ID {request.InvoiceId} not found.");
            }

            invoice.Pay();

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
