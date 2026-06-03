using SubscriptionBillingSystem.Domain.Common;
using SubscriptionBillingSystem.Domain.Exceptions;

namespace SubscriptionBillingSystem.Domain.Aggregates.Customer
{
    public class Customer : Entity, IAggregateRoot
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Address { get; private set; }

        private Customer() { } 

        public Customer(string firstName, string lastName, string email, string address)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new DomainException("First name is required.");
            if (string.IsNullOrWhiteSpace(lastName)) throw new DomainException("Last name is required.");
            if (string.IsNullOrWhiteSpace(address)) throw new DomainException("Address is required.");

            if (!email.Contains("@")) throw new DomainException("Invalid email format.");

            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Address = address;
        }
    }
}
