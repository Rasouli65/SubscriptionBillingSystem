using SubscriptionBillingSystem.Domain.Aggregates.Subscription;

namespace SubscriptionBillingSystem.Domain.Repositories
{    
    public interface ISubscriptionRepository
    {
        Task<Subscription?> GetByIdAsync(Guid id);
        Task AddAsync(Subscription subscription);
        Task<List<Subscription>> GetExpiredSubscriptionsAsync();
    }
}
