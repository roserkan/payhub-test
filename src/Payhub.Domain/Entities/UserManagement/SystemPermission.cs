using Shared.Domain;

namespace Payhub.Domain.Entities.UserManagement;

public class SystemPermission: BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string PermissionGroup { get; set; } = string.Empty;
    public string? Description { get; set; }
}