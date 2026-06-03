using SubscriptionBillingSystem.Domain.Aggregates.Customer;

namespace SubscriptionBillingSystem.Domain.Repositories
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer customer);
        Task<Customer> GetByIdAsync(Guid id);
    }
}
