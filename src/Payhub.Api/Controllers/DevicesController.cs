using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Features.Devices.Commands.Create;
using Payhub.Application.Features.Devices.Commands.Delete;
using Payhub.Application.Features.Devices.Commands.Update;
using Payhub.Application.Features.Devices.Queries.GetAll;
using Shared.CrossCuttingConcerns.Authorization;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DevicesController : ControllerBase
{
    private readonly IMediator  _mediator;
    
    public DevicesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [HasPermission(["device-list"])]
    public async Task<IActionResult> GetAll([FromQuery]GetAllDevicesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost]
    [HasPermission(["device-create"])]
    public async Task<IActionResult> Create([FromBody]CreateDeviceCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    
    [HttpPut]
    [HasPermission(["device-update"])]
    public async Task<IActionResult> Update([FromRoute]int id, [FromBody]UpdateDeviceCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpDelete]
    [HasPermission(["device-delete"])]
    public async Task<IActionResult> Delete([FromRoute]DeleteDeviceCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}