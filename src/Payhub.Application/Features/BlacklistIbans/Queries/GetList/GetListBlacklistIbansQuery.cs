using Payhub.Application.Common.DTOs.BlaklistIbans;
using Shared.Abstractions.Messaging;
using Shared.Utils.Requests;
using Shared.Utils.Responses;

namespace Payhub.Application.Features.BlacklistIbans.Queries.GetList;

public sealed record GetListBlacklistIbansQuery : IQuery<PaginatedResult<BlacklistIbanDto>>
{
    public PageRequest PageRequest { get; set; }
    public BlacklistIbanFilterDto BlacklistIbanFilterDto { get; set; }

    public GetListBlacklistIbansQuery(PageRequest pageRequest, BlacklistIbanFilterDto dto)
    {
        PageRequest = pageRequest;
        BlacklistIbanFilterDto = dto;
    }
}