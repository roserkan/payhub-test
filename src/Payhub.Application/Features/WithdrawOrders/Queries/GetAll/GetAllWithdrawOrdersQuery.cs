using Payhub.Application.Common.DTOs.WithdrawOrders;
using Shared.Abstractions.Messaging;
using Shared.Utils.Requests;
using Shared.Utils.Responses;

namespace Payhub.Application.Features.WithdrawOrders.Queries.GetAll;

public sealed record GetAllWithdrawOrdersQuery : IQuery<PaginatedResult<WithdrawOrderDto>>
{
    public PageRequest PageRequest { get; set; }
    public WithdrawOrderFilterDto WithdrawOrderFilterDto { get; set; }
    
    public GetAllWithdrawOrdersQuery(PageRequest pageRequest, WithdrawOrderFilterDto withdrawOrderFilterDto)
    {
        PageRequest = pageRequest;
        WithdrawOrderFilterDto = withdrawOrderFilterDto;
    }

}