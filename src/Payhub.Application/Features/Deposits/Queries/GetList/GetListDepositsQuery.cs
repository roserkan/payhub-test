using Payhub.Application.Common.DTOs.Deposits;
using Shared.Abstractions.Messaging;
using Shared.Utils.Requests;
using Shared.Utils.Responses;

namespace Payhub.Application.Features.Deposits.Queries.GetList;

public sealed record GetListDepositsQuery : IQuery<PaginatedResult<DepositDto>>
{
    public PageRequest PageRequest { get; set; }
    public DepositFilterDto DepositFilterDto { get; set; }
    
    public GetListDepositsQuery(PageRequest pageRequest, DepositFilterDto depositFilterDto)
    {
        PageRequest = pageRequest;
        DepositFilterDto = depositFilterDto;
    }
}