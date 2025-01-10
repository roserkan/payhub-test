using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Users.Commands.Update;

public sealed record UpdateUserCommand : ICommand<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Username { get; set; } = null!;
    public bool IsTwoFactorEnabled { get; set; }
    public int RoleId { get; set; }
}