using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Common.DTOs.Customers;
using Payhub.Application.Features.Customers.Queries.GetList;
using Shared.CrossCuttingConcerns.Authorization;
using Shared.Utils.Requests;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IMediator  _mediator;
    
    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [HasPermission(["customer-list"])]
    public async Task<IActionResult> GetList([FromQuery]PageRequest pageRequest, [FromQuery]CustomerFilterDto dto)
    {
        var result = await _mediator.Send(new GetListCustomersQuery(pageRequest, dto));
        return Ok(result);
    }
}