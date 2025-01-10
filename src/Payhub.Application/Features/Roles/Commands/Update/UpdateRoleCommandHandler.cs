using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Payhub.Domain.Entities.UserManagement;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.Roles.Commands.Update;

public sealed class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateRoleCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    
    public async Task<int> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.RoleRepository.GetAsync(i => i.Id == request.Id, cancellationToken: cancellationToken);
        if (role == null)
            throw new NotFoundException(ErrorMessages.Role_NotFound);
        
        role.Name = request.Name;
        role.RoleType = request.RoleType;
        
        await _unitOfWork.RoleRepository.UpdateAsync(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return role.Id;
    }
}