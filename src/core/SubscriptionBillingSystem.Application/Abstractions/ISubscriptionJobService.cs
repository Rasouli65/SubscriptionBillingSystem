namespace SubscriptionBillingSystem.Application.Abstractions
{
    public interface ISubscriptionJobService
    {
        Task ProcessExpiredSubscriptions();
    }
}
