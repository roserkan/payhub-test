using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Features.AffiliateSafeMoves.Commands.Create;
using Payhub.Application.Features.AffiliateSafeMoves.Commands.Delete;
using Payhub.Application.Features.AffiliateSafeMoves.Queries.GetAll;
using Shared.CrossCuttingConcerns.Authorization;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AffiliateSafeMovesController : ControllerBase
{
    private readonly IMediator  _mediator;
    
    public AffiliateSafeMovesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [HasPermission(["affiliate-safe-move-list"])]
    public async Task<IActionResult> GetAll([FromQuery]GetAllAffiliateSafeMovesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost]
    [HasPermission(["affiliate-safe-move-in"])]
    public async Task<IActionResult> Create([FromBody]CreateAffiliateSafeMoveCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    
    [HttpDelete("{id}")]
    [HasPermission(["affiliate-safe-move-out"])]
    public async Task<IActionResult> Delete([FromRoute]int id)
    {
        var command = new DeleteAffiliateSafeMoveCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}