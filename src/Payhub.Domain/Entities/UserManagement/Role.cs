using Payhub.Domain.Enums;
using Shared.Domain;

namespace Payhub.Domain.Entities.UserManagement;

public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public RoleType? RoleType { get; set; } // Admin, Manager, User
    
    // Navigation properties
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RoleSystemPermission> RoleSystemPermissions { get; set; } = new List<RoleSystemPermission>();
    public ICollection<RoleSitePermission> RoleSitePermissions { get; set; } = new List<RoleSitePermission>();
    public ICollection<RoleAffiliatePermission> RoleAffiliatePermissions { get; set; } = new List<RoleAffiliatePermission>();
}