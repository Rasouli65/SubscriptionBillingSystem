using Microsoft.EntityFrameworkCore;
using SubscriptionBillingSystem.Domain.Aggregates.Subscription;
using SubscriptionBillingSystem.Domain.Repositories;

namespace SubscriptionBillingSystem.Infrastructure.Persistence.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionRepository(ApplicationDbContext context) => _context = context;

        public async Task AddAsync(Subscription subscription)
        {
            await _context.Subscriptions.AddAsync(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task<Subscription?> GetByIdAsync(Guid id)
            => await _context.Subscriptions.FindAsync(id);

        public async Task<List<Subscription>> GetExpiredSubscriptionsAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Subscriptions
                .Where(s => s.Status == SubscriptionStatus.Active
                         && s.ExpirationDate != null
                         && s.ExpirationDate < now)
                .ToListAsync();
        }
    }
}
