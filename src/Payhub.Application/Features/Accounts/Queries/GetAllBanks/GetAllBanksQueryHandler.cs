using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.DTOs.Accounts;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Accounts.Queries.GetAllBanks;

public sealed class GetAllBanksQueryHandler : IQueryHandler<GetAllBanksQuery, IEnumerable<BankDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetAllBanksQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<BankDto>> Handle(GetAllBanksQuery request, CancellationToken cancellationToken)
    {
        var banks = await _unitOfWork.BankRepository.GetAllWithSelectorAsync(
            selector: b => new BankDto
            {
                Id = b.Id,
                Name = b.Name,
                IconUrl = b.IconUrl
            }, cancellationToken: cancellationToken);
        
        return banks;
    }
}