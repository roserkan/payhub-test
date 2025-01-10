using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Users.Commands.Create;

public sealed record CreateUserCommand : ICommand<int>
{
    public string Name { get; set; } = String.Empty;
    public string Username { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public int RoleId { get; set; }
}