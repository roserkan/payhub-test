using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Features.PaymentWays.Queries.GetAll;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentWaysController : ControllerBase
{
    private readonly IMediator  _mediator;
    
    public PaymentWaysController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]GetAllPaymentWaysQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}