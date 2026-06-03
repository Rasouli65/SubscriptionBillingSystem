using SubscriptionBillingSystem.Domain.Aggregates.Invoice;
using Microsoft.EntityFrameworkCore;
using SubscriptionBillingSystem.Domain.Repositories;

namespace SubscriptionBillingSystem.Infrastructure.Persistence.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Invoice?> GetByIdAsync(Guid id)
        {
            return await _context.Invoices.FindAsync(id);
        }

        public async Task AddAsync(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
        }

        public async Task<IEnumerable<Invoice>> GetByCustomerIdAsync(Guid customerId)
        {
            return await _context.Invoices
                .Where(i => i.CustomerId == customerId)
                .ToListAsync();
        }

        public void Update(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
        }
    }
}
