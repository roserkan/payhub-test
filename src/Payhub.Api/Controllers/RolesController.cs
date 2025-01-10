using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Features.Roles.Commands.AssignAffiliatePermission;
using Payhub.Application.Features.Roles.Commands.AssignSitePermission;
using Payhub.Application.Features.Roles.Commands.AssignSystemPermission;
using Payhub.Application.Features.Roles.Commands.Create;
using Payhub.Application.Features.Roles.Commands.Delete;
using Payhub.Application.Features.Roles.Commands.Update;
using Payhub.Application.Features.Roles.Queries.GetAll;
using Payhub.Application.Features.Roles.Queries.GetForSelect;
using Shared.CrossCuttingConcerns.Authorization;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IMediator  _mediator;
    
    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [HasPermission(["role-list"])]
    public async Task<IActionResult> GetAll([FromQuery]GetAllRolesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("select")]
    public async Task<IActionResult> GetForSelect([FromQuery]GetRoleForSelectQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost]
    [HasPermission(["role-create"])]
    public async Task<IActionResult> Create([FromBody]CreateRoleCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    
    [HttpPut("{id}")]
    [HasPermission(["role-update"])]
    public async Task<IActionResult> Update([FromRoute]int id, [FromBody]UpdateRoleCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    [HasPermission(["role-delete"])]
    public async Task<IActionResult> Delete([FromRoute]int id)
    {
        var command = new DeleteRoleCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPut("{id}/assign-system-permission")]
    [HasPermission(["role-assign-system-permission"])]
    public async Task<IActionResult> AssignSystemPermission([FromRoute]int id, [FromBody]AssignSystemPermissionCommand command)
    {
        command.RoleId = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPut("{id}/assign-site-permission")]
    [HasPermission(["role-assign-site-permission"])]
    public async Task<IActionResult> AssignSitePermission([FromRoute]int id, [FromBody]AssignSitePermissionCommand command)
    {
        command.RoleId = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPut("{id}/assign-affiliate-permission")]
    [HasPermission(["role-assign-affiliate-permission"])]
    public async Task<IActionResult> AssignAffiliatePermission([FromRoute]int id, [FromBody]AssignAffiliatePermissionCommand command)
    {
        command.RoleId = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}