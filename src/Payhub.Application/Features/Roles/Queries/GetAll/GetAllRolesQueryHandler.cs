using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.DTOs.Roles;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Roles.Queries.GetAll;

public sealed class GetAllRolesQueryHandler : IQueryHandler<GetAllRolesQuery, IEnumerable<RoleDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetAllRolesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _unitOfWork.RoleRepository.GetAllWithSelectorAsync(
            selector: r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                RoleType = r.RoleType,
                SystemPermissions = r.RoleSystemPermissions.Select(x => new SystemPermissionDto
                {
                    Id = x.SystemPermission.Id,
                    Key = x.SystemPermission.Key,
                    Name = x.SystemPermission.Name
                }),
                SitePermissions = r.RoleSitePermissions.Select(x => new SitePermissionDto
                {
                    Id = x.Site.Id,
                    SiteName = x.Site.Name
                }),
                AffiliatePermissions = r.RoleAffiliatePermissions.Select(x => new AffiliatePermissionDto
                {
                    Id = x.Affiliate.Id,
                    AffiliateName = x.Affiliate.Name
                })
            },
            cancellationToken: cancellationToken);
        
        return roles;
    }
}