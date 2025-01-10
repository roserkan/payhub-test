using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Features.Callbacks.Commands.PayoxCallback;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CallbacksController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public CallbacksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> PayoxCallback([FromBody]PayoxCallbackCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}