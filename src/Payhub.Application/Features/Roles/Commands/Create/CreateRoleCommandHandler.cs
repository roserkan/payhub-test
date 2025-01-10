using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.UserManagement;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Roles.Commands.Create;

public sealed class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateRoleCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    
    public async Task<int> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = new Role
        {
            Name = request.Name,
            Description = request.Description,
            RoleType = request.RoleType
        };
        
        await _unitOfWork.RoleRepository.AddAsync(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return role.Id;
    }
}