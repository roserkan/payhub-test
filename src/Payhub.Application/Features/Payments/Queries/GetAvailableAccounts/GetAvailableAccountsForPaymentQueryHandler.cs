using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.DTOs.Payments;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Payments.Queries.GetAvailableAccounts;

public sealed class GetAvailableAccountsForPaymentQueryHandler : IQueryHandler<GetAvailableAccountsForPaymentQuery, IEnumerable<AccountForPaymentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetAvailableAccountsForPaymentQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<AccountForPaymentDto>> Handle(GetAvailableAccountsForPaymentQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.AccountRepository.GetAllWithSelectorAsync(
            predicate: i => i.MinDepositAmount <= request.Amount &&
                 i.MaxDepositAmount >= request.Amount &&
                 i.IsActive &&
                 i.AccountType == AccountType.Yatirim &&
                 i.PaymentWayId == request.PaymentWayId && 
                 i.AccountSites.Any(x => x.SiteId == request.SiteId),
            selector: a => new AccountForPaymentDto
            {
                Id = a.Id,
                AccountName = a.Name,
                AccountNumber = a.AccountNumber,
                BankName = a.Bank.Name,
                BankIconUrl = a.Bank.IconUrl
            }, cancellationToken: cancellationToken);
        
        return result;
    }
}