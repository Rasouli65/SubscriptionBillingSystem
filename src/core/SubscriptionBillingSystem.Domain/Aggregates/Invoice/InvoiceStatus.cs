namespace SubscriptionBillingSystem.Domain.Aggregates.Invoice
{
    public enum InvoiceStatus
    {
        Pending = 1,
        Paid = 2,
        Overdue = 3,
        Cancelled = 4
    }
}
