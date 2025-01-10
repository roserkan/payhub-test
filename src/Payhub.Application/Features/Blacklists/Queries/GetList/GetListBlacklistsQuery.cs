using Payhub.Application.Common.DTOs.Blacklists;
using Payhub.Application.Common.DTOs.Customers;
using Shared.Abstractions.Messaging;
using Shared.Utils.Requests;
using Shared.Utils.Responses;

namespace Payhub.Application.Features.Blacklists.Queries.GetList;

public sealed record GetListBlacklistsQuery : IQuery<PaginatedResult<BlacklistDto>>
{
    public PageRequest PageRequest { get; set; }
    public CustomerFilterDto CustomerFilterDto { get; set; }

    public GetListBlacklistsQuery(PageRequest pageRequest, CustomerFilterDto dto)
    {
        PageRequest = pageRequest;
        CustomerFilterDto = dto;
    }
}