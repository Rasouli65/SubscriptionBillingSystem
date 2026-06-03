using MediatR;

namespace SubscriptionBillingSystem.Application.Customers.Commands
{
    public record CreateCustomerCommand(string FisrtName, string LastName, string Email, string Address) : IRequest<Guid>;
}
