using SubscriptionBillingSystem.Application.Abstractions;
using SubscriptionBillingSystem.Domain.Repositories;

namespace SubscriptionBillingSystem.Infrastructure.BackgroundJobs
{
    public class SubscriptionJobService : ISubscriptionJobService
    {
        private readonly ISubscriptionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionJobService(ISubscriptionRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task ProcessExpiredSubscriptions()
        {
            var expiredSubscriptions = await _repository.GetExpiredSubscriptionsAsync();

            if (!expiredSubscriptions.Any()) return;

            foreach (var subscription in expiredSubscriptions)
            {
                subscription.Expire();
            }

            await _unitOfWork.SaveChangesAsync();

            Console.WriteLine($"[Hangfire] Processed {expiredSubscriptions.Count} expired subscriptions.");
        }
    }
}


