using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Abstractions.Services;
using Payhub.Application.Common.DTOs.Sites;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Sites.Queries.GetAll;

public sealed class GetAllSitesQueryHandler : IQueryHandler<GetAllSitesQuery, IEnumerable<SiteDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPermissionService _permissionService;
    
    public GetAllSitesQueryHandler(IUnitOfWork unitOfWork, IPermissionService permissionService)
    {
        _unitOfWork = unitOfWork;
        _permissionService = permissionService;
    }

    public async Task<IEnumerable<SiteDto>> Handle(GetAllSitesQuery request, CancellationToken cancellationToken)
    {
        var sitePermissions = await _permissionService.GetSitePermissionsAsync();
        
        var sites = await _unitOfWork.SiteRepository.GetAllWithSelectorAsync<SiteDto>(
            predicate: i => request.DisableSiteFilter || sitePermissions.Contains(i.Id),
            selector: s => new SiteDto
            {
                Id = s.Id,
                Name = s.Name,
                Address = s.Address,
                InfraName = s.Infrastructure.Name,
                SitePaymentWays = s.SitePaymentWays.Select(spw => new SpwDto
                {
                    Id = spw.Id,
                    PaymentWayId = spw.PaymentWayId,
                    ApiKey = spw.ApiKey,
                    SecretKey = spw.SecretKey,
                    Commission = spw.Commission,
                    MinBalanceLimit = spw.MinBalanceLimit,
                    MaxBalanceLimit = spw.MaxBalanceLimit,
                    IsActive = spw.IsActive,
                })
            }
            ,cancellationToken: cancellationToken);
        
        return sites;
    }
}