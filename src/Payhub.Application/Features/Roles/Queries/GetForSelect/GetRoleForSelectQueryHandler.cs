using Payhub.Application.Abstractions.Repositories;
using Shared.Abstractions.Messaging;
using Shared.Utils.Responses;

namespace Payhub.Application.Features.Roles.Queries.GetForSelect;

public class GetRoleForSelectQueryHandler : IQueryHandler<GetRoleForSelectQuery, IEnumerable<SelectDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetRoleForSelectQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    
    public async Task<IEnumerable<SelectDto>> Handle(GetRoleForSelectQuery request, CancellationToken cancellationToken)
    {
        var roles = await _unitOfWork.RoleRepository.GetAllWithSelectorAsync<SelectDto>(selector: r => new SelectDto
            {
                Id = r.Id,
                Name = r.Name
            },
            cancellationToken: cancellationToken);
        
        return roles;
    }
}