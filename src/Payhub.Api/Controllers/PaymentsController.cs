using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Features.Payments.Commands.Payed;
using Payhub.Application.Features.Payments.Commands.SetDepositAccount;
using Payhub.Application.Features.Payments.Queries.GetAvailableAccounts;
using Payhub.Application.Features.Payments.Queries.GetDepositForPayment;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly IMediator  _mediator;
    
    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("deposit-payment/{paymentId}")]
    public async Task<IActionResult> GetDepositForPayment([FromRoute] string paymentId){
        var query = new GetDepositForPaymentQuery(paymentId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("available-accounts")]
    public async Task<IActionResult> GetAvailableAccounts([FromQuery] GetAvailableAccountsForPaymentQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("deposit/set-account")]
    public async Task<IActionResult> SetDepositAccount([FromQuery] SetDepositAccountCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPut("payed/deposit/{id}")]
    public async Task<IActionResult> Payed([FromRoute] int id)
    {
        var command = new DepositPayedCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}