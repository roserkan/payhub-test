using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Features.HavaleBots.Commands.Login;
using Payhub.Application.Features.HavaleBots.Commands.Trigger;
using Payhub.Application.Features.HavaleBots.Queries.GetCurrentAccount;
using Payhub.Application.Features.HavaleBots.Queries.GetDeviceId;
using Shared.CrossCuttingConcerns.Authorization;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Api.Controllers;

[Route("api/havale-bot")]
[ApiController]
public class HavaleBotsController : ControllerBase
{
    private readonly IMediator  _mediator;
    
    public HavaleBotsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] HavaleBotLoginCommand command)
    {
        HttpContext.Request.Headers.TryGetValue("x-bot-key", out var botKey);
        if (botKey != "a2915bd9-1394-4b75-87d5-570e9f82e8a3")
            throw new AuthorizationException("Key is not valid");
        
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("get-device-id")]
    [IsHavBotCheck]
    public async Task<IActionResult> GetDeviceId([FromQuery]HavaleBotGetDeviceIdQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("get-current-account")]
    [IsHavBotCheck]
    public async Task<IActionResult> GetCurrentAccount([FromQuery]HavaleBotGetCurrentAccountQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    // Trigger
    [HttpGet("trigger")]
    public async Task<IActionResult> Trigger(HavaleBotTriggerCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}