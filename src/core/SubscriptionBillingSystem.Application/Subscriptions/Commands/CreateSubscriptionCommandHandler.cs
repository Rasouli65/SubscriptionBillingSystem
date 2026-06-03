using MediatR;
using SubscriptionBillingSystem.Application.Abstractions;
using SubscriptionBillingSystem.Domain.Aggregates.Subscription;
using SubscriptionBillingSystem.Domain.Repositories;
using SubscriptionBillingSystem.Domain.ValueObjects;

namespace SubscriptionBillingSystem.Application.Subscriptions.Commands
{
    public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, Guid>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSubscriptionCommandHandler(
            ISubscriptionRepository subscriptionRepository,
            IUnitOfWork unitOfWork)
        {
            _subscriptionRepository = subscriptionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var price = Money.Create(request.Amount, request.Currency);
            var subscription = new Subscription(request.CustomerId, price);

            subscription.Activate();

            await _subscriptionRepository.AddAsync(subscription);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return subscription.Id;
        }
    }
}
