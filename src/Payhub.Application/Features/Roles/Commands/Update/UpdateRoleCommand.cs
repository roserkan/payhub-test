using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Roles.Commands.Update;

public sealed record UpdateRoleCommand : ICommand<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public RoleType? RoleType { get; set; }
}