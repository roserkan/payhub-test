using Microsoft.EntityFrameworkCore;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Payhub.Domain.Entities.UserManagement;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.Roles.Commands.AssignAffiliatePermission;

public sealed class AssignAffiliatePermissionCommandHandler : ICommandHandler<AssignAffiliatePermissionCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public AssignAffiliatePermissionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(AssignAffiliatePermissionCommand request, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.RoleRepository.GetAsync(i => i.Id == request.RoleId, cancellationToken: cancellationToken,
            include: i => i.Include(x => x.RoleAffiliatePermissions),
            enableTracking: true);
        
        if (role == null)
            throw new NotFoundException(ErrorMessages.Role_NotFound);
        
        role.RoleAffiliatePermissions.Clear();
        
        foreach (var affiliateId in request.AffiliateIdList)
        {
            role.RoleAffiliatePermissions.Add(new RoleAffiliatePermission
            {
                RoleId = role.Id,
                AffiliateId = affiliateId
            });
        }

        await _unitOfWork.RoleRepository.UpdateAsync(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return role.Id;
    }
}