using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Abstractions.Services;
using Payhub.Domain.Entities.SafeManagement;
using Payhub.Domain.Entities.AffiliateManagement;
using Payhub.Domain.Entities.UserManagement;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.AffiliateSafeMoves.Queries.GetAll;

public sealed class GetAllAffiliateSafeMovesQueryHandler : IQueryHandler<GetAllAffiliateSafeMovesQuery, IEnumerable<AffiliateSafeMove>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPermissionService _permissionService;
    
    public GetAllAffiliateSafeMovesQueryHandler(IUnitOfWork unitOfWork, IPermissionService permissionService)
    {
        _unitOfWork = unitOfWork;
        _permissionService = permissionService;
    }

    public async Task<IEnumerable<AffiliateSafeMove>> Handle(GetAllAffiliateSafeMovesQuery request, CancellationToken cancellationToken)
    {
        var affiliateIdList = await _permissionService.GetAffiliatePermissionsAsync();
        
        Expression<Func<AffiliateSafeMove, bool>>? predicate = affiliateSafeMove =>
            affiliateSafeMove.TransactionDate >= request.StartDateSettedTime &&
            affiliateSafeMove.TransactionDate <= request.EndDateSettedTime &&
            affiliateIdList.Contains(affiliateSafeMove.AffiliateId);
        
        var affiliateSafes = await _unitOfWork.AffiliateSafeMoveRepository.GetAllWithSelectorAsync(
            predicate: predicate,
            selector: s => new AffiliateSafeMove
            {
                Id = s.Id,
                AffiliateId = s.AffiliateId,
                MoveType = s.MoveType,
                TransactionType = s.TransactionType,
                Amount = s.Amount,
                CommissionAmount = s.CommissionAmount,
                CommissionRate = s.CommissionRate,
                Description = s.Description,
                TransactionDate = s.TransactionDate,
                CreatedDate = s.CreatedDate,
                Affiliate = new Affiliate
                {
                    Id = s.Affiliate.Id,
                    Name = s.Affiliate.Name,
                },
                CreatedUser = new User
                {
                    Id = s.CreatedUser.Id,
                    Name = s.CreatedUser.Name,
                }
            },
            cancellationToken: cancellationToken
        );
        
        return affiliateSafes;
    }
}

