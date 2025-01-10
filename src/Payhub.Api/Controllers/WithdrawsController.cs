using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Common.DTOs.Withdraws;
using Payhub.Application.Features.Withdraws.Commands.Create;
using Payhub.Application.Features.Withdraws.Commands.CreateForAccount;
using Payhub.Application.Features.Withdraws.Commands.Update;
using Payhub.Application.Features.Withdraws.Commands.UpdateStatus;
using Payhub.Application.Features.Withdraws.Queries.GetDetailForAccount;
using Payhub.Application.Features.Withdraws.Queries.GetList;
using Payhub.Domain.Enums;
using Shared.CrossCuttingConcerns.Authorization;
using Shared.Utils.Requests;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WithdrawsController : ControllerBase
{
    private readonly IMediator  _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public WithdrawsController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }
   
    [HttpGet]
    [HasPermission(["withdraw-list"])]
    public async Task<IActionResult> GetList([FromQuery]PageRequest pageRequest, [FromQuery]WithdrawFilterDto dto)
    {
        var query = new GetListWithdrawsQuery(pageRequest, dto);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody]CreateWithdrawCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("create-auto")]
    public async Task<IActionResult> CreateAuto([FromBody]CreateWithdrawForAccountCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpGet("auto/detail/{processId}")]
    public async Task<IActionResult> GetDetailForAuto([FromRoute]string processId)
    {
        var result = await _mediator.Send(new GetWithdrawDetailForAccountQuery() { ProcessId = processId});
        return Ok(result);
    }
    
    [HttpPut("{id}")]
    [HasPermission(["withdraw-update"])]
    public async Task<IActionResult> Update([FromQuery]int id, [FromBody]UpdateWithdrawCommand command)
    {
        if (!CheckRemoteIp())
            return Unauthorized();
        
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus([FromQuery]int id, [FromBody]UpdateWithdrawStatusCommand command)
    {
        if (!CheckStatusPermissions(command.Status))
            return Unauthorized();
        
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    private bool CheckRemoteIp()
    {
        var remoteIpAddress = _httpContextAccessor.HttpContext?.Connection.LocalIpAddress;

        if (remoteIpAddress == null)
            return false;

        // İzin verilen IP adreslerini tanımlayın
        var allowedIps = new List<string>
        {
            "162.19.253.44",
            "51.195.218.141"
        };

        // IPv4 formatına dönüştürüp karşılaştırma yapıyoruz
        var remoteIpString = remoteIpAddress.MapToIPv4().ToString();
        return allowedIps.Contains(remoteIpString);
    }


    private bool CheckStatusPermissions(WithdrawStatus status)
    {
        var user = _httpContextAccessor.HttpContext.User;
        if (!user.Identity.IsAuthenticated)
            return false;
        
        var userPermissions = user.Claims
            .Where(c => c.Type == "permission")
            .Select(c => c.Value)
            .ToList();

        if (status == WithdrawStatus.Confirmed && !userPermissions.Any(p => p == "withdraw-confirm"))
            return false;
        
        if (status == WithdrawStatus.Declined && !userPermissions.Any(p => p == "withdraw-decline"))
            return false;
        
        if (status == WithdrawStatus.PendingWithdraw && !userPermissions.Any(p => p == "withdraw-transfer-to-awaiting"))
            return false;

        return true;
    }
}