using MediatR;
using Microsoft.AspNetCore.Mvc;
using SubscriptionBillingSystem.Application.Customers.Commands;
using SubscriptionBillingSystem.Application.Customers.Queries;

namespace SubscriptionBillingSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CustomersController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { CustomerId = id });
        }

        [HttpGet("{customerId}/subscriptions")]
        public async Task<IActionResult> GetSubscriptions(Guid customerId)
        {
            var result = await _mediator.Send(new GetCustomerSubscriptionsQuery(customerId));

            if (result == null || !result.Any())
                return NotFound($"No subscriptions found for customer: {customerId}");

            return Ok(result);
        }
    }
}
