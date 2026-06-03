using SubscriptionBillingSystem.Domain.Aggregates.Invoice;

namespace SubscriptionBillingSystem.Domain.Repositories
{
    public interface IInvoiceRepository
    {
        Task<Invoice?> GetByIdAsync(Guid id);
        Task AddAsync(Invoice invoice);
        Task<IEnumerable<Invoice>> GetByCustomerIdAsync(Guid customerId);

        void Update(Invoice invoice);
    }
}
