using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.WithdrawOrders.Commands.Delete;

public sealed class DeleteWithdrawOrderCommandHandler : ICommandHandler<DeleteWithdrawOrderCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteWithdrawOrderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(DeleteWithdrawOrderCommand request, CancellationToken cancellationToken)
    {
        var device = await _unitOfWork.WithdrawOrders.GetAsync(i => i.Id == request.Id, cancellationToken: cancellationToken);
        if (device == null)
            throw new NotFoundException(ErrorMessages.WithdrawOrder_NotFound);
        
        await _unitOfWork.WithdrawOrders.DeleteAsync(device);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return device.Id;
    }
}