using Microsoft.EntityFrameworkCore;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Abstractions.Services;
using Payhub.Application.Common.DTOs.Analysis;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Analysis.Queries.AffiliateAnalysis;

public sealed class GetAffiliateAnalysisQueryHandler : IQueryHandler<GetAffiliateAnalysisQuery, AffiliateAnalysResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPermissionService _permissionService;
    
    public GetAffiliateAnalysisQueryHandler(IUnitOfWork unitOfWork, IPermissionService permissionService)
    {
        _unitOfWork = unitOfWork;
        _permissionService = permissionService;
    }
    
    
    public async Task<AffiliateAnalysResponseDto> Handle(GetAffiliateAnalysisQuery request, CancellationToken cancellationToken)
    {
        var affiliateIdList = await _permissionService.GetAffiliatePermissionsAsync();

        // Veritabanında tüm hesaplamaları yapacak şekilde sorguyu düzenleyin
        var affiliates = await _unitOfWork.AffiliateRepository.Query()
            .Where(s => affiliateIdList.Contains(s.Id))
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

                CommissionAmount = (s.Deposits
                    .Where(d => d.Status == DepositStatus.Confirmed &&
                                d.TransactionDate >= request.StartDateSettedTime &&
                                d.TransactionDate <= request.EndDateSettedTime)
                    .Sum(d => (decimal?)((d.PayedAmount * d.Commission) / 100)) ?? 0) - (s.Deposits
                    .Where(d => d.Status == DepositStatus.Confirmed &&
                                d.TransactionDate >= request.StartDateSettedTime &&
                                d.TransactionDate <= request.EndDateSettedTime)
                    .Sum(d => (decimal?)((d.PayedAmount * d.AffiliateCommission) / 100)) ?? 0),
                
                AffiliateCommissionAmount = s.Deposits
                    .Where(d => d.Status == DepositStatus.Confirmed &&
                                d.TransactionDate >= request.StartDateSettedTime &&
                                d.TransactionDate <= request.EndDateSettedTime)
                    .Sum(d => (decimal?)((d.PayedAmount * d.AffiliateCommission) / 100)) ?? 0,

                ExternalInAmount = s.AffiliateSafeMoves
                    .Where(ss => ss.MoveType == AffiliateSafeMoveType.In && ss.TransactionDate >= request.StartDateSettedTime && ss.TransactionDate <= request.EndDateSettedTime)
                    .Sum(ss => (decimal?)ss.Amount) ?? 0,

                ExternalOutAmount = s.AffiliateSafeMoves
                    .Where(ss => ss.MoveType == AffiliateSafeMoveType.Out && ss.TransactionDate >= request.StartDateSettedTime && ss.TransactionDate <= request.EndDateSettedTime)
                    .Sum(ss => (decimal?)ss.Amount) ?? 0,

                ExternalCommissionAmount = s.AffiliateSafeMoves
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
                        .Sum(d => (decimal?)(Convert.ToDouble(d.PayedAmount) * (Convert.ToDouble(d.AffiliateCommission) / 100.0)) ?? 0))
                    +
                    // AffiliateSafeMoves IN, OUT ve Komisyon toplamları
                    ((s.AffiliateSafeMoves
                         .Where(ss => ss.MoveType == AffiliateSafeMoveType.In && ss.TransactionDate <= request.EndDateSettedTime)
                         .Sum(ss => ss.Amount))
                     - 
                    (s.AffiliateSafeMoves
                        .Where(ss => ss.MoveType == AffiliateSafeMoveType.Out && ss.TransactionDate <= request.EndDateSettedTime)
                        .Sum(ss => ss.Amount))
                    -
                    (s.AffiliateSafeMoves
                        .Where(ss => ss.TransactionDate <= request.EndDateSettedTime)
                        .Sum(ss => ss.CommissionAmount)))

            })
            .ToListAsync(cancellationToken);

        var affiliateAnalysisList = affiliates.Select(affiliate => new AffiliateAnalysDto
        {
            Id = affiliate.Id,
            Name = affiliate.Name,
            
            DepositAmount = affiliate.DepositAmount,
            DepositCount = affiliate.DepositCount,
            
            WithdrawAmount = affiliate.WithdrawAmount,
            WithdrawCount = affiliate.WithdrawCount,
            
            CommissionAmount = affiliate.CommissionAmount,
            AffiliateCommissionAmount = affiliate.AffiliateCommissionAmount,
            
            ExternalInAmount = affiliate.ExternalInAmount,
            ExternalOutAmount = affiliate.ExternalOutAmount,
            
            ExternalCommissionAmount = affiliate.ExternalCommissionAmount,
            
            Balance = affiliate.Balance
        }).ToList();

        return new AffiliateAnalysResponseDto
        {
            AffiliateAnalysis = affiliateAnalysisList,
            AffiliateAnalysisAll = new AffiliateAnalysAllDto
            {
                TotalDepositAmount = affiliateAnalysisList.Sum(i => i.DepositAmount),
                TotalDepositCount = affiliateAnalysisList.Sum(i => i.DepositCount),
                TotalWithdrawAmount = affiliateAnalysisList.Sum(i => i.WithdrawAmount),
                TotalWithdrawCount = affiliateAnalysisList.Sum(i => i.WithdrawCount),
                TotalCommission = affiliateAnalysisList.Sum(i => i.CommissionAmount) + affiliateAnalysisList.Sum(i => i.ExternalCommissionAmount),
                AffiliateTotalCommission = affiliateAnalysisList.Sum(i => i.AffiliateCommissionAmount) + affiliateAnalysisList.Sum(i => i.ExternalCommissionAmount),
                TotalBalance = affiliateAnalysisList.Sum(i => i.Balance),
            }
        };
    }
}