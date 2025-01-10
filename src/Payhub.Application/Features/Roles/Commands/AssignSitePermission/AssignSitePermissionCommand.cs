using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Roles.Commands.AssignSitePermission;

public sealed record AssignSitePermissionCommand : ICommand<int>
{
    public int RoleId { get; set; }
    public List<int> SiteIdList { get; set; } = new();
}