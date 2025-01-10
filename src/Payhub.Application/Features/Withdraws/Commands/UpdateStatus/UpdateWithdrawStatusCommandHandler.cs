using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Abstractions.Services;
using Payhub.Application.Common.Constants;
using Payhub.Application.Common.DTOs.Infra;
using Payhub.Domain.Enums;
using Shared.Abstractions.Hubs;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;
using Shared.Utils.Security.Extensions;
using Shared.Utils.Security.Hashing;

namespace Payhub.Application.Features.Withdraws.Commands.UpdateStatus;

public sealed class UpdateWithdrawStatusCommandHandler : ICommandHandler<UpdateWithdrawStatusCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionStatusService _transactionStatusService;
    
    public UpdateWithdrawStatusCommandHandler(IUnitOfWork unitOfWork, ITransactionStatusService transactionStatusService)
    {
        _unitOfWork = unitOfWork;
        _transactionStatusService = transactionStatusService;
    }
    
    public async Task<int> Handle(UpdateWithdrawStatusCommand request, CancellationToken cancellationToken)
    {
        var withdraw = await _unitOfWork.WithdrawRepository.GetAsync(i => i.Id == request.WithdrawId, cancellationToken: cancellationToken,
            include: i => i.Include(x => x.Site)
                .ThenInclude(x => x.SitePaymentWays)
                .Include(x => x.Site)
                .ThenInclude(x => x.Infrastructure),
            enableTracking: true);

        await _transactionStatusService.UpdateWithdrawStatusAsync(withdraw, request.Status,
            request.SendToInfra,  request.AccountId,null, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return withdraw!.Id;
    }
}