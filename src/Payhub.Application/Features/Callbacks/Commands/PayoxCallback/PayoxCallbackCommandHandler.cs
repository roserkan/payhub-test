using Microsoft.EntityFrameworkCore;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Abstractions.Services;
using Payhub.Application.Common.DTOs.Callbacks;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.Callbacks.Commands.PayoxCallback;

public sealed class PayoxCallbackCommandHandler : ICommandHandler<PayoxCallbackCommand, CallbackReceivedDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionStatusService _transactionStatusService;
    
    public PayoxCallbackCommandHandler(IUnitOfWork unitOfWork, ITransactionStatusService transactionStatusService)
    {
        _unitOfWork = unitOfWork;
        _transactionStatusService = transactionStatusService;
    }
    
    public async Task<CallbackReceivedDto> Handle(PayoxCallbackCommand request, CancellationToken cancellationToken)
    {
        if (request.Type == "deposit")
        {
            var deposit = await _unitOfWork.DepositRepository.GetAsync(i => i.ProcessId == request.ProcessId, cancellationToken: cancellationToken,
                include: i => i.Include(x => x.Site)
                    .ThenInclude(x => x.SitePaymentWays)
                    .Include(x => x.Site)
                    .ThenInclude(x => x.Infrastructure),
                enableTracking: true);
            if (deposit == null)
                throw new BusinessException($"{request.ProcessId} işlemd Id'li deposit bulunamadı.");
            
            deposit.PayedAmount = request.Amount;
            
            if (deposit.Status == DepositStatus.PendingConfirmation ||
                deposit.Status == DepositStatus.PendingDeposit)
            {
                if (request.Status == "successful")
                {
                    await _transactionStatusService.UpdateDepositStatusAsync(deposit: deposit, 
                        status: DepositStatus.Confirmed, 
                        sendToInfra: true,
                        updatedName: "AUTO",
                        cancellationToken: cancellationToken);
                }
                   
                else
                {
                    var isTimeOut = request.StatusReason?.Contains("Zaman") ?? false;
                    if (isTimeOut)
                    {
                        await _transactionStatusService.UpdateDepositStatusAsync(deposit: deposit, 
                            status: DepositStatus.TimeOut, 
                            sendToInfra: true,
                            updatedName: "AUTO",
                            cancellationToken: cancellationToken);
                    }
                    else
                    {
                        await _transactionStatusService.UpdateDepositStatusAsync(deposit: deposit, 
                            status: DepositStatus.Declined, 
                            sendToInfra: true,
                            updatedName: "AUTO",
                            cancellationToken: cancellationToken);
                    }
                }
            }

        }

        if (request.Type == "withdrawal")
        {
            var withdraw = await _unitOfWork.WithdrawRepository.GetAsync(i => i.ProcessId == request.ProcessId, cancellationToken: cancellationToken,
                include: i => i.Include(x => x.Site)
                    .ThenInclude(x => x.SitePaymentWays)
                    .Include(x => x.Site)
                    .ThenInclude(x => x.Infrastructure),
                enableTracking: true);
            
            if (withdraw == null)
                throw new BusinessException($"{request.ProcessId} işlemd Id'li withdraw bulunamadı.");

            withdraw!.PayedAmount = request.Amount;

            if (withdraw.Status == WithdrawStatus.PendingWithdraw)
            {
                if (request.Status == "successful")
                {
                    await _transactionStatusService.UpdateWithdrawStatusAsync(withdraw: withdraw, 
                        status: WithdrawStatus.Confirmed, 
                        sendToInfra: true,
                        accountId: 6, // todo:hard-coded: PAYOX DINAMI CEKIM HESAP ID
                        updatedName: "AUTO",
                        cancellationToken: cancellationToken);;
                }
                    
                else
                {
                    var isTimeOut = request.StatusReason?.Contains("Zaman") ?? false;
                    if (isTimeOut)
                    {
                       
                    }
                    else
                    {
                        await _transactionStatusService.UpdateWithdrawStatusAsync(withdraw: withdraw, 
                            status: WithdrawStatus.Declined, 
                            sendToInfra: true,
                            accountId: withdraw.AccountId,
                            updatedName: "AUTO",
                            cancellationToken: cancellationToken);;
                    }
                }
            }
        
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new CallbackReceivedDto("Callback received", true);
    }
}