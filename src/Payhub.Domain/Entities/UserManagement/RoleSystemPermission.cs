using Shared.Domain;

namespace Payhub.Domain.Entities.UserManagement;

public class RoleSystemPermission: BaseEntity
{
    public int RoleId { get; set; }
    public int SystemPermissionId { get; set; }
    
    // Navigation properties
    public Role Role { get; set; } = null!;
    public SystemPermission SystemPermission { get; set; } = null!;
}