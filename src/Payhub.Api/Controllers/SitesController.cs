using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Features.Sites.Commands.Create;
using Payhub.Application.Features.Sites.Commands.Update;
using Payhub.Application.Features.Sites.Queries.GetAll;
using Payhub.Application.Features.Sites.Queries.GetForSelect;
using Shared.CrossCuttingConcerns.Authorization;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SitesController : ControllerBase
{
    private readonly IMediator  _mediator;
    
    public SitesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [HasPermission(["site-list"])]
    public async Task<IActionResult> GetAll([FromQuery]GetAllSitesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("select")]
    public async Task<IActionResult> GetForSelect([FromQuery]GetSiteForSelectQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost]
    [HasPermission(["site-create"])]
    public async Task<IActionResult> Create([FromBody]CreateSiteCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    
    [HttpPut("{id}")]
    [HasPermission(["site-update"])]
    public async Task<IActionResult> Update([FromRoute]int id, [FromBody]UpdateSiteCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}