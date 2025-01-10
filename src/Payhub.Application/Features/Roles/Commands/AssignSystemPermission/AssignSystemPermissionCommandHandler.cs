using Microsoft.EntityFrameworkCore;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Payhub.Domain.Entities.UserManagement;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.Roles.Commands.AssignSystemPermission;

public sealed class AssignSystemPermissionCommandHandler : ICommandHandler<AssignSystemPermissionCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public AssignSystemPermissionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(AssignSystemPermissionCommand request, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.RoleRepository.GetAsync(i => i.Id == request.RoleId, cancellationToken: cancellationToken,
            include: i => i.Include(x => x.RoleSystemPermissions),
            enableTracking: true);
        
        if (role == null)
            throw new NotFoundException(ErrorMessages.Role_NotFound);
        
        role.RoleSystemPermissions.Clear();
        
        foreach (var permissionId in request.PermissionIdList)
        {
            role.RoleSystemPermissions.Add(new RoleSystemPermission
            {
                RoleId = role.Id,
                SystemPermissionId = permissionId,
            });
        }

        await _unitOfWork.RoleRepository.UpdateAsync(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return role.Id;
    }
}