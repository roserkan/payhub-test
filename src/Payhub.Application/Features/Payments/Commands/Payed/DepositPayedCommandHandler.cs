using Microsoft.AspNetCore.SignalR;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Payhub.Domain.Enums;
using Shared.Abstractions.Hubs;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.Payments.Commands.Payed;

public sealed class DepositPayedCommandHandler : ICommandHandler<DepositPayedCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<NotificationHub> _hubContext;
    
    public DepositPayedCommandHandler(IUnitOfWork unitOfWork, IHubContext<NotificationHub> hubContext)
    {
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
    }

    public async Task<int> Handle(DepositPayedCommand request, CancellationToken cancellationToken)
    {
        var deposit = await _unitOfWork.DepositRepository.GetAsync(i => i.Id == request.DepositId, cancellationToken: cancellationToken);
        if (deposit is null)
            throw new NotFoundException(ErrorMessages.Deposits_NotFound);

        if (deposit.Status == DepositStatus.PendingDeposit) 
            deposit.Status = DepositStatus.PendingConfirmation;
        
        await _unitOfWork.DepositRepository.UpdateAsync(deposit);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "deposit");

        return deposit.Id;
    }
}