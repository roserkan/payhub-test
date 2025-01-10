using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Features.Permissions.GetAllSystemPermissionsForSelect;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SystemPermissionsController : ControllerBase
{
    private readonly IMediator  _mediator;
    
    public SystemPermissionsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("select")]
    public async Task<IActionResult> GetForSelect([FromQuery]GetAllSystemPermissionsForSelect query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}