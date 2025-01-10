using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Features.Devices.Commands.Create;
using Payhub.Application.Features.Devices.Commands.Delete;
using Payhub.Application.Features.Devices.Commands.Update;
using Payhub.Application.Features.Devices.Queries.GetAll;
using Payhub.Application.Features.HavaleBotMoves.Commands;
using Shared.CrossCuttingConcerns.Authorization;

namespace Payhub.Api.Controllers;

[Route("api/havale-bot-moves")]
[ApiController]
public class HavaleBotMovesController : ControllerBase
{
    private readonly IMediator  _mediator;
    
    public HavaleBotMovesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [IsHavBotCheck]
    public async Task<IActionResult> Create([FromBody]CreateHavaleBotMoveCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}