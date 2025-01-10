using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Features.SiteSafeMoves.Commands.Create;
using Payhub.Application.Features.SiteSafeMoves.Commands.Delete;
using Payhub.Application.Features.SiteSafeMoves.Queries.GetAll;
using Shared.CrossCuttingConcerns.Authorization;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SiteSafeMovesController : ControllerBase
{
    private readonly IMediator  _mediator;
    
    public SiteSafeMovesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [HasPermission(["site-safe-move-list"])]
    public async Task<IActionResult> GetAll([FromQuery]GetAllSiteSafeMovesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost]
    [HasPermission(["site-safe-move-in"])]
    public async Task<IActionResult> Create([FromBody]CreateSiteSafeMoveCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    
    [HttpDelete("{id}")]
    [HasPermission(["site-safe-move-out"])]
    public async Task<IActionResult> Delete([FromRoute]int id)
    {
        var command = new DeleteSiteSafeMoveCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}