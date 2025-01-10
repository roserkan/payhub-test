using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Features.Analysis.Queries.AccountAnalysis;
using Payhub.Application.Features.Analysis.Queries.AffiliateAnalysis;
using Payhub.Application.Features.Analysis.Queries.DailyAnalysis;
using Payhub.Application.Features.Analysis.Queries.SiteAnalysis;
using Shared.CrossCuttingConcerns.Authorization;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnalysisController : ControllerBase
{
    private readonly IMediator  _mediator;
    
    public AnalysisController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("site-analysis")]
    [HasPermission(["site-safe-state-list"])]
    public async Task<IActionResult> SiteAnalysis([FromQuery]GetSiteAnalysisQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("affiliate-analysis")]
    [HasPermission(["affiliate-safe-state-list"])]
    public async Task<IActionResult> AffiliateAnalysis([FromQuery]GetAffiliateAnalysisQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    
    [HttpGet("account-analysis")]
    [HasPermission(["account-safe-state-list"])]
    public async Task<IActionResult> AccountAnalysis([FromQuery]GetAccountAnalysisQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    
    [HttpPost("daily-analysis")]
    public async Task<IActionResult> DailyAnalysis([FromBody]GetDailyAnalysisQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}