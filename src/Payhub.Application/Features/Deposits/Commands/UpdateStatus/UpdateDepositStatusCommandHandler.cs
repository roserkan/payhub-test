using Microsoft.EntityFrameworkCore;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Abstractions.Services;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Deposits.Commands.UpdateStatus;

public sealed class UpdateDepositStatusCommandHandler : ICommandHandler<UpdateDepositStatusCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionStatusService _transactionStatusService;
    
    public UpdateDepositStatusCommandHandler(IUnitOfWork unitOfWork, ITransactionStatusService transactionStatusService)
    {
        _unitOfWork = unitOfWork;
        _transactionStatusService = transactionStatusService;
    }
    
    public async Task<int> Handle(UpdateDepositStatusCommand request, CancellationToken cancellationToken)
    {
        var deposit = await _unitOfWork.DepositRepository.GetAsync(i => i.Id == request.DepositId, cancellationToken: cancellationToken,
            include: i => i.Include(x => x.Site)
                .ThenInclude(x => x.SitePaymentWays)
                .Include(x => x.Site)
                .ThenInclude(x => x.Infrastructure),
            enableTracking: true);
        
        
        await _transactionStatusService.UpdateDepositStatusAsync(deposit, request.Status,
            request.SendToInfra, null, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return deposit!.Id;
    }
}