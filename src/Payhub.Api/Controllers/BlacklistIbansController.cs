using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Common.DTOs.BlaklistIbans;
using Payhub.Application.Features.BlacklistIbans.Commands.Create;
using Payhub.Application.Features.BlacklistIbans.Commands.Delete;
using Payhub.Application.Features.BlacklistIbans.Queries.GetList;
using Shared.CrossCuttingConcerns.Authorization;
using Shared.Utils.Requests;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlacklistIbansController : ControllerBase
{
    private readonly IMediator  _mediator;
    
    public BlacklistIbansController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [HasPermission(["blacklist-iban-list"])]
    public async Task<IActionResult> GetList([FromQuery]PageRequest pageRequest, [FromQuery]BlacklistIbanFilterDto dto)
    {
        var result = await _mediator.Send(new GetListBlacklistIbansQuery(pageRequest, dto));
        return Ok(result);
    }
    
    [HttpPost]
    [HasPermission(["blacklist-iban-create"])]
    public async Task<IActionResult> Create([FromBody]CreateBlacklistIbanCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    [HasPermission(["blacklist-iban-delete"])]
    public async Task<IActionResult> Delete([FromRoute]int id)
    {
        var result = await _mediator.Send(new DeleteBlacklistIbanCommand() {Id = id});
        return Ok(result);
    }
}