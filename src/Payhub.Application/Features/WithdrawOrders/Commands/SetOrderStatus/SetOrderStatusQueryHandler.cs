using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Payhub.Application.Features.HavaleBots.BotResults;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.WithdrawOrders.Commands.SetOrderStatus;

public sealed class SetOrderStatusQueryHandler : ICommandHandler<SetOrderStatusQuery, BotResult<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public SetOrderStatusQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BotResult<int>> Handle(SetOrderStatusQuery request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.WithdrawOrders.GetAsync(i => i.Id == request.BankRobotTransferOrderId,
            cancellationToken: cancellationToken);
        if (order == null)
            throw new NotFoundException(ErrorMessages.WithdrawOrder_NotFound);

        order.Status = request.Status!.Value;
        
        await _unitOfWork.WithdrawOrders.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BotResult<int>.Ok(order.Id);
    }
}