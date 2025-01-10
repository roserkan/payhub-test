using MediatR;
using Microsoft.AspNetCore.Mvc;
using OtpNet;
using Payhub.Application.Common.DTOs.Users;
using Payhub.Application.Features.Users.Commands.Login;
using Payhub.Application.Features.Users.Commands.LoginTwoFactor;
using Payhub.Application.Features.Users.Queries.CurrentUser;
using Payhub.Infrastructure.Persistence.Contexts;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator  _mediator;
    private readonly ApplicationDbContext _context;
    
    public AuthController(IMediator mediator, ApplicationDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }
    
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var query = new GetCurrentUserQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("verify-2fa")]
    public async Task<IActionResult> VerifyTwoFactor([FromBody] TwoFactorDto dto)
    {
        var secretKey = _context.Users.FirstOrDefault(u => u.Username == dto.Username)?.TwoFactorSecret;
        if (secretKey is null)
            return new UnauthorizedResult();

        var totp = new Totp(Base32Encoding.ToBytes(secretKey));
        var verify =  totp.VerifyTotp(dto.Code, out _, new VerificationWindow(2, 2));

        if (verify)
        {
            var result = await _mediator.Send(new LoginTwoFactorCommand { Username = dto.Username });
            return Ok(result);
        }

        return Ok(false);
    }
}