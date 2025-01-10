using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.DTOs.Withdraws;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Withdraws.Queries.GetDetailForAccount;

public sealed class GetWithdrawDetailForAccountQueryHandler : IQueryHandler<GetWithdrawDetailForAccountQuery, WithdrawDetailForAccountDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetWithdrawDetailForAccountQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<WithdrawDetailForAccountDto> Handle(GetWithdrawDetailForAccountQuery request, CancellationToken cancellationToken)
    {
        var withdraw = await _unitOfWork.WithdrawRepository.GetWithSelectorAsync(i => i.ProcessId == request.ProcessId,
            selector: d => new WithdrawDetailForAccountDto
            { 
                Id = d.Id,
                ProcessId = d.ProcessId,
                Amount = d.Amount,
                PayedAmount = d.PayedAmount,
                Status = d.Status,
                InfraConfirmed = d.InfraConfirmed,
                AccountName = d.CustomerAccountNumber, // todo: check this
                Iban = d.CustomerAccountNumber
            });
         
        return withdraw!;
    }
}