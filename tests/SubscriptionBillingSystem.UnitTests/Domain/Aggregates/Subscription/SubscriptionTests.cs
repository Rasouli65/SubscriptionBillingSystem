using FluentAssertions;
using SubscriptionEntity = SubscriptionBillingSystem.Domain.Aggregates.Subscription;
using SubscriptionBillingSystem.Domain.ValueObjects;
using SubscriptionBillingSystem.Domain.Aggregates.Subscription;

namespace SubscriptionBillingSystem.UnitTests.Domain.Aggregates.Subscription
{
    public class SubscriptionTests
    {
        [Fact]
        public void Constructor_ShouldInitializeCorrectly()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var price = Money.Create(100.00m, "USD");

            // Act
            var subscription = new SubscriptionEntity.Subscription(customerId, price);

            // Assert
            subscription.Id.Should().NotBeEmpty();
            subscription.CustomerId.Should().Be(customerId);
            subscription.Price.Should().Be(price);
            subscription.Status.Should().Be(SubscriptionEntity.SubscriptionStatus.Pending);
            subscription.Invoices.Should().BeEmpty();
        }

        [Fact]
        public void Activate_ShouldChangeStatusToActive_And_RaiseDomainEvent()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var price = Money.Create(100.00m, "USD");
            var subscription = new SubscriptionEntity.Subscription(customerId, price);

            // Act
            subscription.Activate();

            // Assert
            subscription.Status.Should().Be(SubscriptionEntity.SubscriptionStatus.Active);
            subscription.ActivatedAt.Should().NotBeNull();

            subscription.DomainEvents.Should().ContainSingle(e => e is SubscriptionActivatedDomainEvent)
                .Which.Should().BeOfType<SubscriptionActivatedDomainEvent>()
                .Which.As<SubscriptionActivatedDomainEvent>().SubscriptionId.Should().Be(subscription.Id);
        }

        [Fact]
        public void Cancel_ShouldChangeStatusToCancelled()
        {
            // Arrange
            var subscription = new SubscriptionEntity.Subscription(Guid.NewGuid(), Money.Create(10, "USD"));

            // Act
            subscription.Cancel();

            // Assert
            subscription.Status.Should().Be(SubscriptionEntity.SubscriptionStatus.Cancelled);
        }

        [Fact]
        public void Activate_WhenAlreadyActive_ShouldNotRaiseDuplicateEvents()
        {
            // Arrange
            var subscription = new SubscriptionEntity.Subscription(Guid.NewGuid(), Money.Create(10, "USD"));
            subscription.Activate();
            var initialEventCount = subscription.DomainEvents.Count;

            // Act
            subscription.Activate();

            // Assert
            subscription.DomainEvents.Count.Should().Be(initialEventCount);
        }
    }
}
