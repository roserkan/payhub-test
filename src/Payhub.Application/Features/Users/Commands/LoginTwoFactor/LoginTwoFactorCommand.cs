using Payhub.Application.Common.DTOs.Users;
using Payhub.Application.Common.Pipelines.Logging;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Users.Commands.LoginTwoFactor;

public sealed record LoginTwoFactorCommand : ICommand<VerifiedLoggedDto>, ILogRequest
{
    public string Username { get; set; }
}