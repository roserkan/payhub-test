using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Users.Commands.ResetPassword;

public sealed record ResetPasswordCommand : ICommand<int>
{
    public string Username { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string OldPassword { get; set; } = string.Empty;
}