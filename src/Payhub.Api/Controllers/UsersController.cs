using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Features.Users.Commands.Create;
using Payhub.Application.Features.Users.Commands.Delete;
using Payhub.Application.Features.Users.Commands.ResetPassword;
using Payhub.Application.Features.Users.Commands.Update;
using Payhub.Application.Features.Users.Queries.GetAll;
using Shared.CrossCuttingConcerns.Authorization;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator  _mediator;
    
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [HasPermission(["user-list"])]
    public async Task<IActionResult> GetAll([FromQuery]GetAllUsersQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost]
    [HasPermission(["user-create"])]
    public async Task<IActionResult> Create([FromBody]CreateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    
    [HttpPut("{id}")]
    [HasPermission(["user-update"])]
    public async Task<IActionResult> Update([FromRoute]int id, [FromBody]UpdateUserCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPut("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    [HasPermission(["user-delete"])]
    public async Task<IActionResult> Delete([FromRoute]int id)
    {
        var command = new DeleteUserCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}