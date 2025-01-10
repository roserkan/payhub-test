using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.DTOs.Affiliates;
using Shared.Abstractions.Messaging;
using Shared.Utils.Responses;

namespace Payhub.Application.Features.Affiliates.Queries.GetAll;

public sealed class GetAllAffiliatesQueryHandler : IQueryHandler<GetAllAffiliatesQuery, IEnumerable<AffiliateDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetAllAffiliatesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<AffiliateDto>> Handle(GetAllAffiliatesQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.AffiliateRepository.GetAllWithSelectorAsync<AffiliateDto>(
            selector: af => new AffiliateDto
            {
                Id = af.Id,
                Name = af.Name,
                IsDynamic = af.IsDynamic,
                DailyDepositLimit = af.DailyDepositLimit,
                DailyWithdrawLimit = af.DailyWithdrawLimit,
                MinDepositAmount = af.MinDepositAmount,
                MaxDepositAmount = af.MaxDepositAmount,
                MinWithdrawAmount = af.MinWithdrawAmount,
                MaxWithdrawAmount = af.MaxWithdrawAmount,
                IsDepositActive = af.IsDepositActive,
                IsWithdrawActive = af.IsWithdrawActive,
                DepositLimitExceeded = af.DepositLimitExceeded,
                WithdrawLimitExceeded = af.WithdrawLimitExceeded,
                CommissionRate = af.CommissionRate,
                CreatedDate = af.CreatedDate,
                Sites = af.AffiliateSites.Select(s => new SelectDto
                {
                    Id = s.Site.Id,
                    Name = s.Site.Name
                })
            }, cancellationToken: cancellationToken);
        
        return result;
    }
}