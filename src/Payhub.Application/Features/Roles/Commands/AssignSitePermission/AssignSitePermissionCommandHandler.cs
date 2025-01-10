using Microsoft.EntityFrameworkCore;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Payhub.Domain.Entities.UserManagement;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.Roles.Commands.AssignSitePermission;

public sealed class AssignSitePermissionCommandHandler : ICommandHandler<AssignSitePermissionCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public AssignSitePermissionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(AssignSitePermissionCommand request, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.RoleRepository.GetAsync(i => i.Id == request.RoleId, cancellationToken: cancellationToken,
            include: i => i.Include(x => x.RoleSitePermissions),
            enableTracking: true);
        
        if (role == null)
            throw new NotFoundException(ErrorMessages.Role_NotFound);
        
        role.RoleSitePermissions.Clear();
        
        foreach (var siteId in request.SiteIdList)
        {
            role.RoleSitePermissions.Add(new RoleSitePermission
            {
                RoleId = role.Id,
                SiteId = siteId
            });
        }

        await _unitOfWork.RoleRepository.UpdateAsync(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return role.Id;
    }
}