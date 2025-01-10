using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.DTOs.WithdrawOrders;
using Payhub.Application.Features.HavaleBots.BotResults;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.WithdrawOrders.Queries.GetPendings;

public sealed class GetPendingOrderQueryHandler : IQueryHandler<GetPendingOrderQuery, BotResult<PendingWithdrawOrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetPendingOrderQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<BotResult<PendingWithdrawOrderDto>> Handle(GetPendingOrderQuery request, CancellationToken cancellationToken)
    {
        var query = await _unitOfWork.WithdrawOrders
            .GetWithSelectorAsync(i => i.AccountId == request.AccId && i.Status == WithdrawOrderStatus.Pending, 
                selector: ow => new PendingWithdrawOrderDto()
                {
                    Id = ow.Id,
                    ReceiverAccountNumber = ow.ReceiverAccountNumber,
                    ReceiverAccountFullName = ow.ReceiverFullName,
                    Description = ow.Description,
                    Amount = ow.Amount,
                    SenderAccountId = ow.Account.Id,
                    IsCompleted = ow.Status == WithdrawOrderStatus.Success,
                    BankName = ow.Account.Bank.Name
                },
                cancellationToken: cancellationToken);

        if (query is null)
            return BotResult<PendingWithdrawOrderDto>.Ok(null!);
        
        return BotResult<PendingWithdrawOrderDto>.Ok(query);
    }
}