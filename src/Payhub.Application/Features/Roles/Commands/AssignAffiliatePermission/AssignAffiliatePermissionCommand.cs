using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Roles.Commands.AssignAffiliatePermission;

public sealed record AssignAffiliatePermissionCommand : ICommand<int>
{
    public int RoleId { get; set; }
    public List<int> AffiliateIdList { get; set; } = new();
}