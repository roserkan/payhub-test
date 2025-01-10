using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Abstractions.Services;
using Shared.Abstractions.Messaging;
using Shared.Utils.Responses;

namespace Payhub.Application.Features.Sites.Queries.GetForSelect;

public sealed class GetSiteForSelectQueryHandler : IQueryHandler<GetSiteForSelectQuery, IEnumerable<SelectDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPermissionService _permissionService;
    
    public GetSiteForSelectQueryHandler(IUnitOfWork unitOfWork, IPermissionService permissionService)
    {
        _unitOfWork = unitOfWork;
        _permissionService = permissionService;
    }

    public async Task<IEnumerable<SelectDto>> Handle(GetSiteForSelectQuery request, CancellationToken cancellationToken)
    {
        var sitePermissions = await _permissionService.GetSitePermissionsAsync();
        
        var site = await _unitOfWork.SiteRepository.GetAllWithSelectorAsync<SelectDto>(
            predicate: i => request.DisableSiteFilter || sitePermissions.Contains(i.Id),
            selector: r => new SelectDto
            {
                Id = r.Id,
                Name = r.Name
            },
            cancellationToken: cancellationToken);
        
        return site;
    }
}