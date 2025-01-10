using Payhub.Application.Abstractions.Repositories;
using Shared.Abstractions.Messaging;
using Shared.Utils.Responses;

namespace Payhub.Application.Features.Affiliates.Queries.GetForSelect;

public sealed class GetAffiliatesForSelectQueryHandler : IQueryHandler<GetAffiliatesForSelectQuery, IEnumerable<SelectDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetAffiliatesForSelectQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<SelectDto>> Handle(GetAffiliatesForSelectQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.AffiliateRepository.GetAllWithSelectorAsync<SelectDto>(
            selector: af => new SelectDto
            {
                Id = af.Id,
                Name = af.Name
            }, cancellationToken: cancellationToken);
        
        return result;
    }
}