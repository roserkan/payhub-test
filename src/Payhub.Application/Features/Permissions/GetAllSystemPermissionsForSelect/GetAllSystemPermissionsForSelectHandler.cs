using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.UserManagement;
using Shared.Abstractions.Messaging;
using Shared.Utils.Responses;

namespace Payhub.Application.Features.Permissions.GetAllSystemPermissionsForSelect;

public sealed class GetAllSystemPermissionsForSelectHandler : IQueryHandler<GetAllSystemPermissionsForSelect, IEnumerable<SystemPermission>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetAllSystemPermissionsForSelectHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<SystemPermission>> Handle(GetAllSystemPermissionsForSelect request, CancellationToken cancellationToken)
    {
        var permissions = await _unitOfWork.SystemPermissionRepository.GetAllAsync(cancellationToken: cancellationToken);
        
        return permissions;
    }
}