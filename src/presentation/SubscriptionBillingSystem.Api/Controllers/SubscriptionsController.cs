using MediatR;
using Microsoft.AspNetCore.Mvc;
using SubscriptionBillingSystem.Application.Invoices.Commands.PayInvoice;
using SubscriptionBillingSystem.Application.Invoices.Queries.GetInvoicesBySubscription;
using SubscriptionBillingSystem.Application.Subscriptions.Commands;

namespace SubscriptionBillingSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SubscriptionsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(CreateSubscriptionCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { SubscriptionId = id });
        }

        [HttpPost("invoices/{invoiceId}/pay")]
        public async Task<IActionResult> PayInvoice(Guid invoiceId)
        {
            await _mediator.Send(new PayInvoiceCommand(invoiceId));
            return Ok();
        }

        [HttpGet("{id}/invoices")]
        public async Task<ActionResult<List<InvoiceDto>>> GetInvoices(Guid id)
        {
            var query = new GetInvoicesQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
