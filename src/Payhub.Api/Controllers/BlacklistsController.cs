using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Common.DTOs.Blacklists;
using Payhub.Application.Common.DTOs.Customers;
using Payhub.Application.Features.Blacklists.Commands.Create;
using Payhub.Application.Features.Blacklists.Commands.Delete;
using Payhub.Application.Features.Blacklists.Queries.GetList;
using Shared.CrossCuttingConcerns.Authorization;
using Shared.Utils.Requests;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlacklistsController : ControllerBase
{
    private readonly IMediator  _mediator;
    
    public BlacklistsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [HasPermission(["customer-blacklist-list"])]
    public async Task<IActionResult> GetList([FromQuery]PageRequest pageRequest, [FromQuery]CustomerFilterDto dto)
    {
        var result = await _mediator.Send(new GetListBlacklistsQuery(pageRequest, dto));
        return Ok(result);
    }
    
    [HttpPost]
    [HasPermission(["customer-add-blacklist"])]
    public async Task<IActionResult> Create([FromBody]CreateBlacklistCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    
    [HttpDelete("{panelUserId}")]
    [HasPermission(["customer-remove-blacklist"])]
    public async Task<IActionResult> Delete([FromRoute]string panelUserId)
    {
        var command = new DeleteBlacklistCommand(panelUserId);
        var result = await _mediator.Send(command );
        return Ok(result);
    }
}