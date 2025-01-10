using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.BotManagement;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.WithdrawOrders.Commands.Create;

public sealed class CreateWithdrawOrderCommandHandler : ICommandHandler<CreateWithdrawOrderCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateWithdrawOrderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateWithdrawOrderCommand request, CancellationToken cancellationToken)
    {
        var device = new WithdrawOrder
        {
            AccountId = request.AccountId,
            ReceiverAccountNumber = request.ReceiverAccountNumber,
            ReceiverFullName = request.ReceiverFullName,
            Amount = request.Amount,
            Status = WithdrawOrderStatus.Pending,
            Description = request.Description,
        };
        
        await _unitOfWork.WithdrawOrders.AddAsync(device);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return device.Id;
    }
}