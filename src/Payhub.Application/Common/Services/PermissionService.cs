using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Abstractions.Services;
using Payhub.Application.Common.Constants;
using Shared.CrossCuttingConcerns.Exceptions.Types;
using Shared.Utils.Security.Extensions;

namespace Payhub.Application.Common.Services;

public class PermissionService : IPermissionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    
    public PermissionService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<int>> GetSitePermissionsAsync()
    {
        var currentUserId = _httpContextAccessor.HttpContext!.User.GetUserId();
        if (currentUserId == null)
            throw new NotFoundException(ErrorMessages.User_NotFound);

        var siteIds = await _unitOfWork.UserRepository.Query()
            .Where(u => u.Id == currentUserId)
            .SelectMany(u => u.UserRoles
                .SelectMany(ur => ur.Role.RoleSitePermissions
                    .Select(rsp => rsp.SiteId)))
            .ToListAsync();
        
        return siteIds;
    }

    public async Task<IEnumerable<int>> GetAffiliatePermissionsAsync()
    {
        var currentUserId = _httpContextAccessor.HttpContext!.User.GetUserId();
        if (currentUserId == null)
            throw new NotFoundException(ErrorMessages.User_NotFound);

        var affiliateIds = await _unitOfWork.UserRepository.Query()
            .Where(u => u.Id == currentUserId)
            .SelectMany(u => u.UserRoles
                .SelectMany(ur => ur.Role.RoleAffiliatePermissions
                    .Select(rsp => rsp.AffiliateId)))
            .ToListAsync();
        
        return affiliateIds;
    }
}