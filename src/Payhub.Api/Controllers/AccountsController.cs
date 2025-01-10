using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Features.Accounts.Commands.Create;
using Payhub.Application.Features.Accounts.Commands.Delete;
using Payhub.Application.Features.Accounts.Commands.UnDelete;
using Payhub.Application.Features.Accounts.Commands.Update;
using Payhub.Application.Features.Accounts.Queries.GetAll;
using Payhub.Application.Features.Accounts.Queries.GetAllBanks;
using Payhub.Application.Features.Accounts.Queries.GetForSelect;
using Payhub.Application.Features.Sites.Commands.DefineSite;
using Shared.CrossCuttingConcerns.Authorization;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IMediator  _mediator;
    
    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [HasPermission(["account-list"])]
    public async Task<IActionResult> GetAll([FromQuery]GetAllAccountsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("select")]
    public async Task<IActionResult> GetForSelect([FromQuery]GetAccountForSelectQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost]
    [HasPermission(["account-create"])]
    public async Task<IActionResult> Create([FromBody]CreateAccountCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    
    [HttpPut("{id}")]
    [HasPermission(["account-update"])]
    public async Task<IActionResult> Update([FromRoute]int id, [FromBody]UpdateAccountCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPut("define-site")]
    [HasPermission(["account-define-site"])]
    public async Task<IActionResult> DefineSite([FromBody]DefineSiteCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpGet("banks")]
    public async Task<IActionResult> GetAll([FromQuery]GetAllBanksQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    [HasPermission(["account-delete"])]
    public async Task<IActionResult> Delete([FromRoute]DeleteAccountCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPut("{id}/undelete")]
    [HasPermission(["account-delete"])]
    public async Task<IActionResult> UnDelete([FromRoute]UnDeleteAccountCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}