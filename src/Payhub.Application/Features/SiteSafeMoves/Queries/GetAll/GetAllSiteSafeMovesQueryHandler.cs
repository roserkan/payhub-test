using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Abstractions.Services;
using Payhub.Domain.Entities.SafeManagement;
using Payhub.Domain.Entities.SiteManagement;
using Payhub.Domain.Entities.UserManagement;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.SiteSafeMoves.Queries.GetAll;

public sealed class GetAllSiteSafeMovesQueryHandler : IQueryHandler<GetAllSiteSafeMovesQuery, IEnumerable<SiteSafeMove>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPermissionService _permissionService;
    
    public GetAllSiteSafeMovesQueryHandler(IUnitOfWork unitOfWork, IPermissionService permissionService)
    {
        _unitOfWork = unitOfWork;
        _permissionService = permissionService;
    }

    public async Task<IEnumerable<SiteSafeMove>> Handle(GetAllSiteSafeMovesQuery request, CancellationToken cancellationToken)
    {
        var siteIdList = await _permissionService.GetSitePermissionsAsync();
        
        Expression<Func<SiteSafeMove, bool>>? predicate = siteSafeMove =>
            siteSafeMove.TransactionDate >= request.StartDateSettedTime &&
            siteSafeMove.TransactionDate <= request.EndDateSettedTime &&
            siteIdList.Contains(siteSafeMove.SiteId);
        
        var siteSafes = await _unitOfWork.SiteSafeMoveRepository.GetAllWithSelectorAsync(
            predicate: predicate,
            selector: s => new SiteSafeMove
            {
                Id = s.Id,
                SiteId = s.SiteId,
                MoveType = s.MoveType,
                TransactionType = s.TransactionType,
                Amount = s.Amount,
                CommissionAmount = s.CommissionAmount,
                CommissionRate = s.CommissionRate,
                Description = s.Description,
                TransactionDate = s.TransactionDate,
                CreatedDate = s.CreatedDate,
                Site = new Site
                {
                    Id = s.Site.Id,
                    Name = s.Site.Name,
                },
                CreatedUser = new User
                {
                    Id = s.CreatedUser.Id,
                    Name = s.CreatedUser.Name,
                }
            },
            cancellationToken: cancellationToken
        );
        
        return siteSafes;
    }
}

