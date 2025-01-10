using Microsoft.EntityFrameworkCore;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.DTOs.Accounts;
using Payhub.Application.Common.DTOs.Analysis;
using Payhub.Domain.Entities.TransactionManagement;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Analysis.Queries.AccountAnalysis;

public sealed class GetAccountAnalysisQueryHandler : IQueryHandler<GetAccountAnalysisQuery, IEnumerable<AccountAnalysisDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetAccountAnalysisQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<AccountAnalysisDto>> Handle(GetAccountAnalysisQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _unitOfWork.AccountRepository.Query()
            .Include(s => Enumerable.Where<Deposit>(s.Deposits, d => d.Status == DepositStatus.Confirmed)) // Yatırımların sadece Confirmed olanlarını dahil ediyoruz
            .Include(s => s.Withdraws.Where(w => w.Status == WithdrawStatus.Confirmed)) // Çekimlerin sadece Confirmed olanlarını dahil ediyoruz
            .Include(b => b.Bank) 
            .ToListAsync(cancellationToken);
        
        var accountAnalysList = new List<AccountAnalysisDto>();

        foreach (var account in accounts)
        {
            var accountAnalys = new AccountAnalysisDto
            {
                Id = account.Id,
                Account = new AccountDto
                {
                    Id = account.Id,
                    Name = account.Name,
                    AccountNumber = account.AccountNumber,
                    Bank = new BankDto
                    {
                        Id = account.Bank.Id,
                        Name = account.Bank.Name,
                        IconUrl = account.Bank.IconUrl
                    }
                },

                // Yatırım Tutarı
                DepositAmount = account.Deposits
                    .Where(d => d.CreatedDate >= request.StartDateSettedTime &&
                                d.CreatedDate <= request.EndDateSettedTime)
                    .Sum(d => d.PayedAmount),

                // Yatırım Adedi
                DepositCount = account.Deposits
                    .Count(d => d.CreatedDate >= request.StartDateSettedTime &&
                                d.CreatedDate <= request.EndDateSettedTime),

                // Çekim Tutarı
                WithdrawAmount = account.Withdraws
                    .Where(w => w.CreatedDate >= request.StartDateSettedTime &&
                                w.CreatedDate <= request.EndDateSettedTime)
                    .Sum(w => w.PayedAmount),

                // Çekim Adedi
                WithdrawCount = account.Withdraws
                    .Count(w => w.CreatedDate >= request.StartDateSettedTime &&
                                w.CreatedDate <= request.EndDateSettedTime),

                // Toplam Komisyon
                CommissionAmount = account.Deposits
                    .Where(d => d.CreatedDate >= request.StartDateSettedTime &&
                                d.CreatedDate <= request.EndDateSettedTime)
                    .Sum(d => (d.PayedAmount * d.Commission) / 100),
            };

            // Bakiye hesaplama (Tüm veriler, belirlenen aralığa değil bitiş tarihinden önce olan veriler)
            accountAnalys.Balance = (account.Deposits
                                         .Where(d => d.CreatedDate <= request.EndDateSettedTime)
                                         .Sum(d => d.PayedAmount) -
                                     account.Withdraws
                                         .Where(w => w.CreatedDate <= request.EndDateSettedTime)
                                         .Sum(w => w.PayedAmount) -
                                     account.Deposits
                                         .Where(d => d.CreatedDate <= request.StartDateSettedTime &&
                                                     d.CreatedDate <= request.EndDateSettedTime)
                                         .Sum(d => (d.PayedAmount * d.Commission) / 100));

            accountAnalysList.Add(accountAnalys);
        }        
        
        return accountAnalysList;
    }
}