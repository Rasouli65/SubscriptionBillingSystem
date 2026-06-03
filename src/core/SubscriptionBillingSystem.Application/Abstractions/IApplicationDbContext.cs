using Microsoft.EntityFrameworkCore;
using SubscriptionBillingSystem.Domain.Aggregates.Invoice;
using SubscriptionBillingSystem.Domain.Aggregates.Subscription;
namespace SubscriptionBillingSystem.Application.Abstractions
{
    public interface IApplicationDbContext
    {
        DbSet<Subscription> Subscriptions { get; }
        DbSet<Invoice> Invoices { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
