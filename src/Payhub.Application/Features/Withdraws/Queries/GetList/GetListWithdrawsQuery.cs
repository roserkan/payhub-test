using Payhub.Application.Common.DTOs.Withdraws;
using Shared.Abstractions.Messaging;
using Shared.Utils.Requests;
using Shared.Utils.Responses;

namespace Payhub.Application.Features.Withdraws.Queries.GetList;

public sealed record GetListWithdrawsQuery : IQuery<PaginatedResult<WithdrawDto>>
{
    public PageRequest PageRequest { get; set; }
    public WithdrawFilterDto WithdrawFilterDto { get; set; }
    
    public GetListWithdrawsQuery(PageRequest pageRequest, WithdrawFilterDto depositFilterDto)
    {
        PageRequest = pageRequest;
        WithdrawFilterDto = depositFilterDto;
    }
}