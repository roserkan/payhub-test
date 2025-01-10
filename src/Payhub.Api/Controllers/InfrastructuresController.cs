using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Features.Infrastructures.Commands.Create;
using Payhub.Application.Features.Infrastructures.Commands.Update;
using Payhub.Application.Features.Infrastructures.Queries.GetAll;
using Payhub.Application.Features.Infrastructures.Queries.GetForSelect;
using Shared.CrossCuttingConcerns.Authorization;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InfrastructuresController : ControllerBase
{
    private readonly IMediator  _mediator;
    
    public InfrastructuresController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [HasPermission(["infrastructure-list"])]
    public async Task<IActionResult> GetAll([FromQuery]GetAllInfrastructuresQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("select")]
    public async Task<IActionResult> GetForSelect([FromQuery]GetInfrastructureForSelectQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost]
    [HasPermission(["infrastructure-create"])]
    public async Task<IActionResult> Create([FromBody]CreateInfrastructureCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    
    [HttpPut("{id}")]
    [HasPermission(["infrastructure-update"])]
    public async Task<IActionResult> Update([FromRoute]int id, [FromBody]UpdateInfrastructureCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}