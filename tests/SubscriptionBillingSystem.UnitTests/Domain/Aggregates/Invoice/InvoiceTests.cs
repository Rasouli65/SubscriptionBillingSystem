using FluentAssertions;
using SubscriptionBillingSystem.Domain.Aggregates.Invoice;
using SubscriptionBillingSystem.Domain.ValueObjects;
using InvoiceEntity = SubscriptionBillingSystem.Domain.Aggregates.Invoice.Invoice;

namespace SubscriptionBillingSystem.UnitTests.Domain.Aggregates.Invoice
{
    public class InvoiceTests
    {
        [Fact]
        public void NewInvoice_ShouldHavePendingStatus_ByDefault()
        {
            // Arrange & Act            
            var money = Money.Create(50, "USD");
            var invoice = new InvoiceEntity(Guid.NewGuid(), Guid.NewGuid(), money);

            // Assert
            invoice.Status.Should().Be(InvoiceStatus.Pending);
        }

        [Fact]
        public void Pay_ShouldChangeStatusToPaid()
        {
            // Arrange
            var money = Money.Create(50, "USD");
            var invoice = new InvoiceEntity(Guid.NewGuid(), Guid.NewGuid(), money);

            // Act
            invoice.Pay();

            // Assert
            invoice.Status.Should().Be(InvoiceStatus.Paid);
            invoice.PaidAt.Should().NotBeNull();
        }
    }
}
