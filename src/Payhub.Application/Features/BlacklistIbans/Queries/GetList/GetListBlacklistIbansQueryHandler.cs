using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.DTOs.BlaklistIbans;
using Shared.Abstractions.Messaging;
using Shared.Utils.Pagination;
using Shared.Utils.Responses;

namespace Payhub.Application.Features.BlacklistIbans.Queries.GetList;

public sealed class GetListBlacklistIbansQueryHandler : IQueryHandler<GetListBlacklistIbansQuery, PaginatedResult<BlacklistIbanDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetListBlacklistIbansQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<PaginatedResult<BlacklistIbanDto>> Handle(GetListBlacklistIbansQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.BlacklistIbanRepository.Query()
            .Select(i => new BlacklistIbanDto()
            {
                Id = i.Id,
                Iban = i.Iban
            });
        
        if (request.BlacklistIbanFilterDto.SearchValue != null)
            query = query.Where(x => x.Iban.Contains(request.BlacklistIbanFilterDto.SearchValue.Trim().Replace(" ", "")));

        var result = await query.ToPaginateAsync(request.PageRequest.Index, request.PageRequest.Size, 0, cancellationToken);
        var paginatedResult = new PaginatedResult<BlacklistIbanDto>
        {
            Items = result.Items,
            Index = result.Index,
            Size = result.Size,
            Count = result.Count,
            Pages = result.Pages,
            HasPrevious = result.HasPrevious,
            HasNext = result.HasNext
        };
        return paginatedResult;
    }

}