using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Common.DTOs.Deposits;
using Payhub.Application.Features.Deposits.Commands.Create;
using Payhub.Application.Features.Deposits.Commands.CreateForAccount;
using Payhub.Application.Features.Deposits.Commands.Update;
using Payhub.Application.Features.Deposits.Commands.UpdateStatus;
using Payhub.Application.Features.Deposits.Queries.GetDetailForAccount;
using Payhub.Application.Features.Deposits.Queries.GetList;
using Payhub.Domain.Enums;
using Shared.CrossCuttingConcerns.Authorization;
using Shared.Utils.Requests;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepositsController : ControllerBase
{
    private readonly IMediator  _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private ILogger<DepositsController> _logger;
    
    public DepositsController(IMediator mediator, IHttpContextAccessor httpContextAccessor, ILogger<DepositsController> logger)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }
    
    [HttpGet]
    [HasPermission(["deposit-list"])]
    public async Task<IActionResult> GetList([FromQuery]PageRequest pageRequest, [FromQuery]DepositFilterDto dto)
    {
        var query = new GetListDepositsQuery(pageRequest, dto);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody]CreateDepositCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("create-auto")]
    public async Task<IActionResult> CreateAuto([FromBody]CreateDepositForAccountCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpGet("auto/detail/{processId}")]
    public async Task<IActionResult> GetDetailForAuto([FromRoute]string processId)
    {
        var result = await _mediator.Send(new GetDepositDetailForAccountQuery() { ProcessId = processId});
        return Ok(result);
    }
    
    [HttpPut("{id}")]
    [HasPermission(["deposit-update"])]
    public async Task<IActionResult> Update([FromQuery]int id, [FromBody]UpdateDepositCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus([FromQuery]int id, [FromBody]UpdateDepositStatusCommand command)
    {
        if (!CheckStatusPermissions(command.Status))
            return Unauthorized();
        
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    private bool CheckStatusPermissions(DepositStatus status)
    {
        var user = _httpContextAccessor.HttpContext.User;
        if (!user.Identity.IsAuthenticated)
            return false;
        
        var userPermissions = user.Claims
            .Where(c => c.Type == "permission")
            .Select(c => c.Value)
            .ToList();

        if (status == DepositStatus.Confirmed && !userPermissions.Any(p => p == "deposit-confirm"))
            return false;
        
        if (status == DepositStatus.Declined && !userPermissions.Any(p => p == "deposit-decline"))
            return false;
        
        if (status == DepositStatus.PendingConfirmation && !userPermissions.Any(p => p == "deposit-transfer-to-awaiting"))
            return false;

        return true;
    }
}

public class IpLog
{
    public string? LocalIp { get; set; }
    public string? RemoteIp { get; set; }
}