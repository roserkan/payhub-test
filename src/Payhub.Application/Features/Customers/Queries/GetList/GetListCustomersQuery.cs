using Payhub.Application.Common.DTOs.Customers;
using Shared.Abstractions.Messaging;
using Shared.Utils.Requests;
using Shared.Utils.Responses;

namespace Payhub.Application.Features.Customers.Queries.GetList;

public sealed record GetListCustomersQuery : IQuery<PaginatedResult<CustomerDto>>
{
    public PageRequest PageRequest { get; set; }
    public CustomerFilterDto CustomerFilterDto { get; set; }

    public GetListCustomersQuery(PageRequest pageRequest, CustomerFilterDto dto)
    {
        PageRequest = pageRequest;
        CustomerFilterDto = dto;
    }
}