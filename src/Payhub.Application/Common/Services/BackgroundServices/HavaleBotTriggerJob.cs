using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Abstractions.Services;
using Payhub.Domain.Enums;

namespace Payhub.Application.Common.Services.BackgroundServices;

public class HavaleBotTriggerJob
{
    private readonly ILogger<HavaleBotTriggerJob> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionStatusService _transactionStatusService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public HavaleBotTriggerJob(ILogger<HavaleBotTriggerJob> logger, 
        IUnitOfWork unitOfWork,
        ITransactionStatusService transactionStatusService, 
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _transactionStatusService = transactionStatusService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task Execute()
    {
        // Burada iş mantığınızı ekleyin
        _logger.LogInformation($"HavBotTriggerJob executed at: {DateTime.Now}");

        var now = DateTime.Now.AddMinutes(-30);
        var localDate = now.ToLocalTime();
        
        // CreatedDate: 18:30
        // Now: 18:35 => 18:05
        // Now: 19:30 => 19:00
        
        var botMoves = await _unitOfWork.HavaleBotMoves.GetAllAsync(
            i => i.Status == HavaleBotMoveStatus.Pending 
                 && i.CreatedDate > localDate,
            enableTracking: true);

        var turkishCulture = new CultureInfo("tr-TR");
        foreach (var botMove in botMoves)
        {
            botMove.TryCount += 1;

            var name = botMove.SenderName.ToLower(turkishCulture).Replace(" ", ""); // TODO: türkçe karakterleri düzelt.
            var amount = botMove.Amount;
            var createdDate = botMove.CreatedDate;

            var deposits = await _unitOfWork.DepositRepository
                .GetAllAsync(i => i.Amount == amount
                               && i.CreatedDate < createdDate
                               && (i.Status == DepositStatus.PendingDeposit || i.Status == DepositStatus.PendingConfirmation),
                    cancellationToken: default,
                    include: i => i.Include(x => x.Site)
                        .ThenInclude(x => x.SitePaymentWays)
                        .Include(x => x.Site)
                        .ThenInclude(x => x.Infrastructure),
                    enableTracking: true);
            
            var currentDeposit = deposits.FirstOrDefault(i => i.CustomerFullName?.ToLower(turkishCulture).Replace(" ", "") == name);
            
            if (currentDeposit != null)
            {
                await _transactionStatusService.UpdateDepositStatusAsync(currentDeposit, DepositStatus.Confirmed, true, 
                    "HavBot",
                    default);
                
                botMove.Status = HavaleBotMoveStatus.Confirmed;
            }
                
        }
        
        await _unitOfWork.SaveChangesAsync();
        
        _logger.LogInformation($"HavBotTriggerJob finished at: {DateTime.Now}");
        
        return;
    }
}
