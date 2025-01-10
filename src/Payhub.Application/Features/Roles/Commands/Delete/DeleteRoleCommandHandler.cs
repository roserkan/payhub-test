using MediatR;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.Roles.Commands.Delete;

public sealed class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteRoleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<int> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.RoleRepository.GetAsync(i => i.Id == request.Id, cancellationToken: cancellationToken);
        if (role == null)
            throw new NotFoundException(ErrorMessages.Role_NotFound);

        await _unitOfWork.RoleRepository.DeleteAsync(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return role.Id;
    }
}