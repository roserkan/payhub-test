using Microsoft.EntityFrameworkCore;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.DTOs.Accounts;
using Payhub.Application.Common.DTOs.Affiliates;
using Payhub.Application.Common.DTOs.PaymentWays;
using Shared.Abstractions.Messaging;
using Shared.Utils.Responses;

namespace Payhub.Application.Features.Accounts.Queries.GetAll;

public sealed class GetAllAccountsQueryHandler : IQueryHandler<GetAllAccountsQuery, IEnumerable<AccountDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetAllAccountsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<AccountDto>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _unitOfWork.AccountRepository.GetAllWithSelectorAsync<AccountDto>(predicate: i => i.PaymentWayId == request.PaymentWayId,
            selector: a => new AccountDto
            {
                Id = a.Id,
                Name = a.Name,
                AccountNumber = a.AccountNumber,
                IsActive = a.IsActive,
                FirstBalance = a.FirstBalance,
                Password = a.Password,
                PhoneNumber = a.PhoneNumber,
                Email = a.Email,
                EmailPassword = a.EmailPassword,
                EmailImapPassword = a.EmailImapPassword,
                MinDepositAmount = a.MinDepositAmount,
                MaxDepositAmount = a.MaxDepositAmount,
                DailyDepositAmountLimit = a.DailyDepositAmountLimit,
                DailyWithdrawAmountLimit = a.DailyWithdrawAmountLimit,
                IsDeleted = a.IsDeleted,
                PaymentWayId = a.PaymentWayId,
                AffiliateId = a.AffiliateId,
                BankId = a.BankId,
                Bank = new BankDto
                {
                    Id = a.Bank.Id,
                    Name = a.Bank.Name,
                    IconUrl = a.Bank.IconUrl
                },
                PaymentWay = new PaymentWayDto
                {
                    Id = a.PaymentWay.Id,
                    Name = a.PaymentWay.Name
                },
                Affiliate = a.Affiliate == null ? null : new AffiliateDto
                {
                    Id = a.Affiliate.Id,
                    Name = a.Affiliate.Name
                },
                AccountType = a.AccountType,
                AccountClassification = a.AccountClassification,
                Sites = a.AccountSites.Select(s => new SelectDto
                {
                    Id = s.Site.Id,
                    Name = s.Site.Name
                })
            },
            cancellationToken: cancellationToken);
        
        return accounts;
    }
}