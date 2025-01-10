using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Roles.Commands.AssignSystemPermission;

public sealed record AssignSystemPermissionCommand : ICommand<int>
{
    public int RoleId { get; set; }
    public List<int> PermissionIdList { get; set; } = new();
}