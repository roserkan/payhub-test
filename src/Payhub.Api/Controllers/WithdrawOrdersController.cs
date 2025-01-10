using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Features.WithdrawOrders.Commands.Create;
using Payhub.Application.Features.WithdrawOrders.Commands.Delete;
using Payhub.Application.Features.WithdrawOrders.Commands.SetOrderStatus;
using Payhub.Application.Features.WithdrawOrders.Commands.Update;
using Payhub.Application.Features.WithdrawOrders.Queries.GetAll;
using Payhub.Application.Features.WithdrawOrders.Queries.GetPendings;
using Payhub.Domain.Enums;
using Shared.CrossCuttingConcerns.Authorization;

namespace Payhub.Api.Controllers;

[Route("api/withdraw-orders")]
[ApiController]
public class WithdrawOrdersController : ControllerBase
{
    private readonly IMediator  _mediator;
    
    public WithdrawOrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [HasPermission(["device-list"])]
    public async Task<IActionResult> GetAll([FromQuery]GetAllWithdrawOrdersQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost]
    [HasPermission(["device-create"])]
    public async Task<IActionResult> Create([FromBody]CreateWithdrawOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    
    [HttpPut]
    [HasPermission(["device-update"])]
    public async Task<IActionResult> Update([FromRoute]int id, [FromBody]UpdateWithdrawOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpDelete]
    [HasPermission(["device-delete"])]
    public async Task<IActionResult> Delete([FromRoute]DeleteWithdrawOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    // for bot
    [HttpGet("pending-list")]
    [IsHavBotCheck]
    public async Task<IActionResult> PendingList([FromQuery]GetPendingOrderQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost("set-order-status-failed")]
    [IsHavBotCheck]
    public async Task<IActionResult> SetOrderFailed([FromBody]SetOrderStatusQuery query)
    {
        query.Status = WithdrawOrderStatus.Failed;
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost("set-order-status-success")]
    [IsHavBotCheck]
    public async Task<IActionResult> SetOrderSuccess([FromBody]SetOrderStatusQuery query)
    {
        query.Status = WithdrawOrderStatus.Success;
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}