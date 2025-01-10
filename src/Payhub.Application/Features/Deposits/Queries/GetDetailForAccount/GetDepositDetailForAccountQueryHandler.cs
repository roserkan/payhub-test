using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Payhub.Application.Common.DTOs.Deposits;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.Deposits.Queries.GetDetailForAccount;

public sealed class GetDepositDetailForAccountQueryHandler : IQueryHandler<GetDepositDetailForAccountQuery, DepositDetailForAccountDto>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetDepositDetailForAccountQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<DepositDetailForAccountDto> Handle(GetDepositDetailForAccountQuery request, CancellationToken cancellationToken)
    {
        var deposit = await _unitOfWork.DepositRepository.GetWithSelectorAsync(i => i.ProcessId == request.ProcessId,
            selector: d => new DepositDetailForAccountDto
            { 
                Id = d.Id,
                ProcessId = d.ProcessId,
                Amount = d.Amount,
                PayedAmount = d.PayedAmount,
                Status = d.Status,
                InfraConfirmed = d.InfraConfirmed,
                AccountName = d.DynamicAccountName ?? d.Account!.Name,
                Iban = d.DynamicAccountNumber ?? d.Account!.AccountNumber
            }, cancellationToken: cancellationToken);

        if (deposit == null)
            throw new NotFoundException(ErrorMessages.Deposits_NotFound);
         
        return deposit;
    }
}