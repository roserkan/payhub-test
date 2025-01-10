using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payhub.Application.Abstractions.Services;
using Payhub.Domain.Enums;
using Payhub.Infrastructure.Persistence.Contexts;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IPermissionService _permissionService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public TransactionsController(ApplicationDbContext context, IPermissionService permissionService, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _permissionService = permissionService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    [HttpGet("pending-counts")]
    public async Task<IActionResult> GetPendingCounts()
    {

        var siteIdList = await _permissionService.GetSitePermissionsAsync();

        var depositCounts = await _context.Deposits
            .AsNoTracking()
            .Where(d => (d.Status == DepositStatus.PendingConfirmation || d.Status == DepositStatus.PendingDeposit) && siteIdList.Contains(d.SiteId))
            .GroupBy(d => d.PaymentWayId)
            .Select(g => new 
            {
                PaymentWayKey = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        var withdrawCounts = await _context.Withdraws
            .AsNoTracking()
            .Where(d => d.Status == WithdrawStatus.PendingWithdraw && siteIdList.Contains(d.SiteId))
            .GroupBy(d => d.PaymentWay.Id)
            .Select(g => new 
            {
                PaymentWayKey = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        var pendingCounts = new PendingCountDto()
        {
            DepositHavaleCount =
                depositCounts.FirstOrDefault(x => x.PaymentWayKey == (int)PaymentWayEnum.Havale)?.Count ?? 0,
            DepositPaparaCount =
                depositCounts.FirstOrDefault(x => x.PaymentWayKey == (int)PaymentWayEnum.Papara)?.Count ?? 0,
            WithdrawHavaleCount =
                withdrawCounts.FirstOrDefault(x => x.PaymentWayKey == (int)PaymentWayEnum.Havale)?.Count ?? 0,
            WithdrawPaparaCount =
                withdrawCounts.FirstOrDefault(x => x.PaymentWayKey == (int)PaymentWayEnum.Papara)?.Count ?? 0,
        };
        
        return Ok(pendingCounts);
    }
    
    
    [HttpGet("redirect-check/{depositId}")]
    public async Task<IActionResult> RedirectCheck(int depositId)
    {
        var deposit = await _context.Deposits
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == depositId);
        if (deposit == null)
            return NotFound();

        var result = new RedirectCheckDto
        {
            Status = deposit.Status,
            RedirectUrl = deposit.RedirectUrl,
            Message = null
        };

        if (deposit.Status == DepositStatus.Confirmed)
            result.Message = "Talep onaylandı, siteye yönlendiriliyorsunuz";
        
        
        if (deposit.Status == DepositStatus.Declined || deposit.Status == DepositStatus.TimeOut)
            result.Message = "Yatırım talebi bulunmadığı için işlem iptal edilmiştir, siteye yönlendiriliyorsunuz";

        return Ok(result);
    }
}

public class PendingCountDto
{
    public int DepositHavaleCount { get; set; }
    public int DepositPaparaCount { get; set; }
    public int WithdrawHavaleCount { get; set; }
    public int WithdrawPaparaCount { get; set; }
}

public class RedirectCheckDto
{
    public DepositStatus Status { get; set; }
    public string? Message { get; set; }
    public string? RedirectUrl { get; set; }
}