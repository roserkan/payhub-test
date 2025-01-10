using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.WithdrawOrders.Commands.Update;

public sealed class UpdateWithdrawOrderCommandHandler : ICommandHandler<UpdateWithdrawOrderCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateWithdrawOrderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(UpdateWithdrawOrderCommand request, CancellationToken cancellationToken)
    {
        var device = await _unitOfWork.WithdrawOrders.GetAsync(i => i.Id == request.Id, cancellationToken: cancellationToken);
        if (device == null)
            throw new NotFoundException(ErrorMessages.WithdrawOrder_NotFound);
        
        device.AccountId = request.AccountId;
        device.ReceiverAccountNumber = request.ReceiverAccountNumber;
        device.ReceiverFullName = request.ReceiverFullName;
        device.Amount = request.Amount;
        device.Description = request.Description;

        if (request.Status != null)
        {
            device.Status = request.Status.Value;
            device.TransactionDate = DateTime.Now;
        }
        
        await _unitOfWork.WithdrawOrders.UpdateAsync(device);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return device.Id;
    }
}