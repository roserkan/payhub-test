using Microsoft.EntityFrameworkCore;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Abstractions.Services;
using Payhub.Application.Common.DTOs.Analysis;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Analysis.Queries.SiteAnalysis;

public sealed class GetSiteAnalysisQueryHandler : IQueryHandler<GetSiteAnalysisQuery, SiteAnalysResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPermissionService _permissionService;
    
    public GetSiteAnalysisQueryHandler(IUnitOfWork unitOfWork, IPermissionService permissionService)
    {
        _unitOfWork = unitOfWork;
        _permissionService = permissionService;
    }
    
    
    public async Task<SiteAnalysResponseDto> Handle(GetSiteAnalysisQuery request, CancellationToken cancellationToken)
    {
        var siteIdList = await _permissionService.GetSitePermissionsAsync();

        // Veritabanında tüm hesaplamaları yapacak şekilde sorguyu düzenleyin
        var sites = await _unitOfWork.SiteRepository.Query()
            .Where(s => siteIdList.Contains(s.Id))
            .Select(s => new
            {
                s.Id,
                s.Name,
                DepositAmount = s.Deposits
                    .Where(d => d.Status == DepositStatus.Confirmed && d.TransactionDate >= request.StartDateSettedTime && d.TransactionDate <= request.EndDateSettedTime)
                    .Sum(d => (decimal?)d.PayedAmount) ?? 0,

                DepositCount = s.Deposits
                    .Count(d => d.Status == DepositStatus.Confirmed && d.TransactionDate >= request.StartDateSettedTime && d.TransactionDate <= request.EndDateSettedTime),
                
                WithdrawAmount = s.Withdraws
                    .Where(w => w.Status == WithdrawStatus.Confirmed && w.TransactionDate >= request.StartDateSettedTime && w.TransactionDate <= request.EndDateSettedTime)
                    .Sum(w => (decimal?)w.PayedAmount) ?? 0,

                WithdrawCount = s.Withdraws
                    .Count(w => w.Status == WithdrawStatus.Confirmed && w.TransactionDate >= request.StartDateSettedTime && w.TransactionDate <= request.EndDateSettedTime),

                CommissionAmount = s.Deposits
                    .Where(d => d.Status == DepositStatus.Confirmed && d.TransactionDate >= request.StartDateSettedTime && d.TransactionDate <= request.EndDateSettedTime)
                    .Sum(d => (decimal?)((d.PayedAmount * d.Commission) / 100)) ?? 0,

                ExternalInAmount = s.SiteSafeMoves
                    .Where(ss => ss.MoveType == SiteSafeMoveType.In && ss.TransactionDate >= request.StartDateSettedTime && ss.TransactionDate <= request.EndDateSettedTime)
                    .Sum(ss => (decimal?)ss.Amount) ?? 0,

                ExternalOutAmount = s.SiteSafeMoves
                    .Where(ss => ss.MoveType == SiteSafeMoveType.Out && ss.TransactionDate >= request.StartDateSettedTime && ss.TransactionDate <= request.EndDateSettedTime)
                    .Sum(ss => (decimal?)ss.Amount) ?? 0,

                ExternalCommissionAmount = s.SiteSafeMoves
                    .Where(ss => ss.TransactionDate >= request.StartDateSettedTime && ss.TransactionDate <= request.EndDateSettedTime)
                    .Sum(ss => (decimal?)ss.CommissionAmount) ?? 0,

                Balance = 
                    // Deposits toplamı (nullable olarak)
                    (s.Deposits.Where(d => d.TransactionDate <= request.EndDateSettedTime && d.Status == DepositStatus.Confirmed)
                        .Sum(d => d.PayedAmount))
                    - 
                    // Withdraws toplamı (nullable olarak)
                    (s.Withdraws.Where(w => w.TransactionDate <= request.EndDateSettedTime && w.Status == WithdrawStatus.Confirmed)
                        .Sum(w => w.PayedAmount))
                     - 
                     // Deposits içindeki komisyon toplamı (nullable olarak)
                    (s.Deposits
                        .Where(d => d.TransactionDate <= request.EndDateSettedTime && d.Status == DepositStatus.Confirmed)
                        .Sum(d => (decimal?)(Convert.ToDouble(d.PayedAmount) * (Convert.ToDouble(d.Commission) / 100.0)) ?? 0))
                    +
                    // SiteSafeMoves IN, OUT ve Komisyon toplamları
                    ((s.SiteSafeMoves
                         .Where(ss => ss.MoveType == SiteSafeMoveType.In && ss.TransactionDate <= request.EndDateSettedTime)
                         .Sum(ss => ss.Amount))
                     - 
                    (s.SiteSafeMoves
                        .Where(ss => ss.MoveType == SiteSafeMoveType.Out && ss.TransactionDate <= request.EndDateSettedTime)
                        .Sum(ss => ss.Amount))
                    -
                    (s.SiteSafeMoves
                        .Where(ss => ss.TransactionDate <= request.EndDateSettedTime)
                        .Sum(ss => ss.CommissionAmount)))

            })
            .ToListAsync(cancellationToken);

        var siteAnalysisList = sites.Select(site => new SiteAnalysDto
        {
            Id = site.Id,
            Name = site.Name,
            
            DepositAmount = site.DepositAmount,
            DepositCount = site.DepositCount,
            
            WithdrawAmount = site.WithdrawAmount,
            WithdrawCount = site.WithdrawCount,
            
            CommissionAmount = site.CommissionAmount,
            
            ExternalInAmount = site.ExternalInAmount,
            ExternalOutAmount = site.ExternalOutAmount,
            
            ExternalCommissionAmount = site.ExternalCommissionAmount,
            
            Balance = site.Balance
        }).ToList();

        return new SiteAnalysResponseDto
        {
            SiteAnalysis = siteAnalysisList,
            SiteAnalysisAll = new SiteAnalysAllDto
            {
                TotalDepositAmount = siteAnalysisList.Sum(i => i.DepositAmount),
                TotalDepositCount = siteAnalysisList.Sum(i => i.DepositCount),
                TotalWithdrawAmount = siteAnalysisList.Sum(i => i.WithdrawAmount),
                TotalWithdrawCount = siteAnalysisList.Sum(i => i.WithdrawCount),
                TotalCommission = siteAnalysisList.Sum(i => i.CommissionAmount) + siteAnalysisList.Sum(i => i.ExternalCommissionAmount),
                TotalBalance = siteAnalysisList.Sum(i => i.Balance),
            }
        };
    }
}