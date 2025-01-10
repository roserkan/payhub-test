using Payhub.Application.Abstractions.Repositories;
using Shared.Abstractions.Messaging;
using Shared.Utils.Responses;

namespace Payhub.Application.Features.Infrastructures.Queries.GetForSelect;

public class GetInfrastructureForSelectQueryHandler : IQueryHandler<GetInfrastructureForSelectQuery, IEnumerable<SelectDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetInfrastructureForSelectQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    
    public async Task<IEnumerable<SelectDto>> Handle(GetInfrastructureForSelectQuery request, CancellationToken cancellationToken)
    {
        var roles = await _unitOfWork.InfrastructureRepository.GetAllWithSelectorAsync<SelectDto>(selector: r => new SelectDto
            {
                Id = r.Id,
                Name = r.Name
            },
            cancellationToken: cancellationToken);
        
        return roles;
    }
}