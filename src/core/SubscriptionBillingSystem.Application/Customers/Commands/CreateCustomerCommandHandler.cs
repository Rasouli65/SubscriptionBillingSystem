using MediatR;
using SubscriptionBillingSystem.Application.Abstractions;
using SubscriptionBillingSystem.Domain.Aggregates.Customer;
using SubscriptionBillingSystem.Domain.Repositories;

namespace SubscriptionBillingSystem.Application.Customers.Commands
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork; 
        } 

        public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer(request.FisrtName, request.LastName, request.Email, request.Address);

            await _customerRepository.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return customer.Id;
        }
    } 
}

