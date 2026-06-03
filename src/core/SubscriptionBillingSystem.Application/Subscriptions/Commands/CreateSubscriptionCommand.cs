using MediatR;

namespace SubscriptionBillingSystem.Application.Subscriptions.Commands
{
    public record CreateSubscriptionCommand(
     Guid CustomerId,
     decimal Amount,
     string Currency) : IRequest<Guid>;
}
