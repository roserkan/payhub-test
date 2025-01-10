using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Payhub.Application.Abstractions.Services;
using Payhub.Application.Common.Constants;
using Payhub.Application.Common.DTOs.Infra;
using Payhub.Domain.Entities.TransactionManagement;
using Payhub.Domain.Enums;
using Shared.Abstractions.Hubs;
using Shared.CrossCuttingConcerns.Exceptions.Types;
using Shared.Utils.Security.Extensions;
using Shared.Utils.Security.Hashing;

namespace Payhub.Application.Common.Services;

public class TransactionStatusService : ITransactionStatusService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IInfraService _infraService;
    private readonly IHubContext<NotificationHub> _hubContext;
    
    public TransactionStatusService(IHttpContextAccessor httpContextAccessor, IInfraService infraService, IHubContext<NotificationHub> hubContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _infraService = infraService;
        _hubContext = hubContext;
    }
    
    public async Task<Deposit> UpdateDepositStatusAsync(Deposit? deposit, DepositStatus status, bool sendToInfra, string? updatedName, CancellationToken cancellationToken)
    {
        if (deposit is null)
            throw new NotFoundException(ErrorMessages.Deposits_NotFound);
        
        deposit.TransactionDate = DateTime.Now;
        deposit.UpdatedUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        
        if (deposit.UpdatedUserId == null)
            deposit.AutoUpdatedName = updatedName;

        if (status == DepositStatus.Confirmed)
        {
            if (deposit.Status == DepositStatus.Confirmed)
                throw new BusinessException(ErrorMessages.Deposits_AlreadyConfirmed);
            if (deposit.Status == DepositStatus.PendingDeposit && updatedName == null) // updateName null ise PANELDEN ELLE ONAYLAMA YAPILIYORDUR
                throw new BusinessException(ErrorMessages.Deposits_StatusMustBePendingConfirmation);
            deposit.Status = DepositStatus.Confirmed;
            if (sendToInfra)
            {
                var spw = deposit!.Site.SitePaymentWays.FirstOrDefault(i => i.PaymentWayId == deposit.PaymentWayId);
                var infraResult = await _infraService.SendDepositCallbackAsync(new InfraDepositCallbackDto
                {
                    CustomerId = deposit.SiteCustomerId,
                    ProcessId = deposit.ProcessId,
                    Amount = deposit.PayedAmount,
                    SecurityKey = HashingHelper.Sha256Hash(spw!.ApiKey + spw.SecretKey + deposit.ProcessId),
                    Status = true
                }, deposit.Site.Infrastructure.DepositAddress);
            
                if (infraResult.Success)
                {
                    deposit.InfraConfirmed = true;
                    deposit.InfraCallbackType = InfraCallbackType.InfraConfirmed;
                    await _hubContext.Clients.All.SendAsync("Redirect", deposit.RedirectUrl, cancellationToken: cancellationToken);
                }
            }
        }
        
        if (status == DepositStatus.Declined)
        {
            if (deposit.Status == DepositStatus.Declined)
                throw new BusinessException(ErrorMessages.Deposits_AlreadyDeclined);
            
            deposit.Status = DepositStatus.Declined;
            if (sendToInfra)    
            {
                var spw = deposit!.Site.SitePaymentWays.FirstOrDefault(i => i.PaymentWayId == deposit.PaymentWayId);
                await _infraService.SendDepositCallbackAsync(new InfraDepositCallbackDto
                {
                    CustomerId = deposit.SiteCustomerId,
                    ProcessId = deposit.ProcessId,
                    Amount = deposit.PayedAmount,
                    SecurityKey = HashingHelper.Sha256Hash(spw!.ApiKey + spw.SecretKey + deposit.ProcessId),
                    Status = false
                }, deposit.Site.Infrastructure.DepositAddress);
            
                deposit.InfraConfirmed = true;
                deposit.InfraCallbackType = InfraCallbackType.InfraDeclined;
            }
        
            await _hubContext.Clients.All.SendAsync("Redirect", deposit.RedirectUrl, cancellationToken: cancellationToken);
        }

        if (status == DepositStatus.PendingConfirmation)
        {
            if (deposit.Status == DepositStatus.PendingConfirmation)
                throw new BusinessException(ErrorMessages.Deposits_AlreadyTransfered);
            if (deposit.Status == DepositStatus.Confirmed)
                throw new BusinessException(ErrorMessages.Deposits_CannotTransferedForConfirmed);
            if (deposit.Status == DepositStatus.TimeOut)
                throw new BusinessException(ErrorMessages.Deposits_CannotTransferedForTimeout);
            deposit.Status = DepositStatus.PendingConfirmation;
        }
        
        if (status == DepositStatus.TimeOut)
        {
            if (deposit.Status == DepositStatus.TimeOut)
                throw new BusinessException(ErrorMessages.Deposits_AlreadyTimeOut);
            deposit.Status = DepositStatus.TimeOut;
        }
        
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "deposit", cancellationToken: cancellationToken);

        return deposit;
    }

    public async Task<Withdraw> UpdateWithdrawStatusAsync(Withdraw? withdraw, WithdrawStatus status, bool sendToInfra, int? accountId, string? updatedName, CancellationToken cancellationToken)
    {
        if (withdraw is null)
            throw new NotFoundException(ErrorMessages.Withdraws_NotFound);
        
        withdraw.TransactionDate = DateTime.Now;
        withdraw.UpdatedUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        withdraw.AccountId = accountId;
        if (withdraw.UpdatedUserId == null)
            withdraw.AutoUpdatedName = updatedName;

        if (status == WithdrawStatus.Confirmed)
        {
            if (withdraw.Status == WithdrawStatus.Confirmed)
                throw new BusinessException(ErrorMessages.Withdraws_AlreadyConfirmed);
            withdraw.Status = WithdrawStatus.Confirmed;
            if (sendToInfra)
            {
                var spw = withdraw!.Site.SitePaymentWays.FirstOrDefault(i => i.PaymentWayId == withdraw.PaymentWayId);
                var infraResult = await _infraService.SendWithdrawCallbackAsync(new InfraWithdrawCallbackDto
                {
                    CustomerId = withdraw.SiteCustomerId,
                    ProcessId = withdraw.ProcessId,
                    Amount = withdraw.PayedAmount,
                    SecurityKey = HashingHelper.Sha256Hash(spw!.ApiKey + spw.SecretKey + withdraw.ProcessId),
                    Status = true
                }, withdraw.Site.Infrastructure.WithdrawAddress);
            
                if (infraResult.Success)
                {
                    withdraw.InfraConfirmed = true;
                    withdraw.InfraCallbackType = InfraCallbackType.InfraConfirmed;
                }
            }
        }
        
        if (status == WithdrawStatus.Declined)
        {
            if (withdraw.Status == WithdrawStatus.Declined)
                throw new BusinessException(ErrorMessages.Withdraws_AlreadyDeclined);
            withdraw.Status = WithdrawStatus.Declined;
            if (sendToInfra)
            {
                var spw = withdraw!.Site.SitePaymentWays.FirstOrDefault(i => i.PaymentWayId == withdraw.PaymentWayId);
                await _infraService.SendWithdrawCallbackAsync(new InfraWithdrawCallbackDto
                {
                    CustomerId = withdraw.SiteCustomerId,
                    ProcessId = withdraw.ProcessId,
                    Amount = withdraw.PayedAmount,
                    SecurityKey = HashingHelper.Sha256Hash(spw!.ApiKey + spw.SecretKey + withdraw.ProcessId),
                    Status = false
                }, withdraw.Site.Infrastructure.WithdrawAddress);
            
                withdraw.InfraConfirmed = true;
                withdraw.InfraCallbackType = InfraCallbackType.InfraDeclined;
            }
        }
        
        // if (status == WithdrawStatus.PendingWithdraw)
        // {
        //     if (withdraw.Status == WithdrawStatus.PendingWithdraw)
        //         throw new BusinessException(ErrorMessages.Withdraws_AlreadyTransfered);
        //     withdraw.Status = WithdrawStatus.PendingWithdraw;
        // }

        if (status == WithdrawStatus.TimeOut)
        {
            if (withdraw.Status == WithdrawStatus.Confirmed)
                throw new BusinessException(ErrorMessages.Withdraws_AlreadyTimeOut);
            withdraw.Status = WithdrawStatus.TimeOut;
        }

        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "withdraw", cancellationToken: cancellationToken);

        return withdraw;
    }
}