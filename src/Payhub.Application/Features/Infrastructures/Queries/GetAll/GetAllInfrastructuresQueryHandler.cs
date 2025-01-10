using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.DTOs.Infrastructures;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Infrastructures.Queries.GetAll;

public sealed class GetAllInfrastructuresQueryHandler : IQueryHandler<GetAllInfrastructuresQuery, IEnumerable<InfrastructureDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetAllInfrastructuresQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<InfrastructureDto>> Handle(GetAllInfrastructuresQuery request, CancellationToken cancellationToken)
    {
        var iInfrastructures = await _unitOfWork.InfrastructureRepository.GetAllWithSelectorAsync(
            selector: r => new InfrastructureDto
            {
                Id = r.Id,
                Name = r.Name,
                Address = r.Address,
                DepositAddress = r.DepositAddress,
                WithdrawAddress = r.WithdrawAddress,
            },
            cancellationToken: cancellationToken);
        
        return iInfrastructures;
    }
}