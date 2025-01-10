using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Roles.Commands.Create;

public sealed record CreateRoleCommand : ICommand<int>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public RoleType? RoleType { get; set; }
}