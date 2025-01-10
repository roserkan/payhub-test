using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Features.Affiliates.Commands.Create;
using Payhub.Application.Features.Affiliates.Commands.DefineSite;
using Payhub.Application.Features.Affiliates.Commands.Update;
using Payhub.Application.Features.Affiliates.Queries.GetAll;
using Payhub.Application.Features.Affiliates.Queries.GetForSelect;
using Shared.CrossCuttingConcerns.Authorization;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AffiliatesController : ControllerBase
{
    private readonly IMediator  _mediator;
    
    public AffiliatesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [HasPermission(["affiliate-list"])]
    public async Task<IActionResult> GetAll([FromQuery]GetAllAffiliatesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("select")]
    public async Task<IActionResult> GetForSelect([FromQuery]GetAffiliatesForSelectQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost]
    [HasPermission(["affiliate-create"])]
    public async Task<IActionResult> Create([FromBody]CreateAffiliateCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    
    [HttpPut("{id}")]
    [HasPermission(["affiliate-update"])]
    public async Task<IActionResult> Update([FromRoute]int id, [FromBody]UpdateAffiliateCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPut("define-site")]
    [HasPermission(["affiliate-define-site"])]
    public async Task<IActionResult> DefineSite([FromRoute]int id, [FromBody]AffiliateDefineSiteCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}