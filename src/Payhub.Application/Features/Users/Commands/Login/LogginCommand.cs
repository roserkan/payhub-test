using Payhub.Application.Common.DTOs.Users;
using Payhub.Application.Common.Pipelines.Logging;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Users.Commands.Login;

public sealed record LoginCommand : ICommand<LoggedDto>, ILogRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}