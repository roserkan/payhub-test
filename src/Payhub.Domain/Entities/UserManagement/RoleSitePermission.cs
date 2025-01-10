using Payhub.Domain.Entities.SiteManagement;
using Shared.Domain;

namespace Payhub.Domain.Entities.UserManagement;

public class RoleSitePermission: BaseEntity
{
    public int RoleId { get; set; }
    public int SiteId { get; set; }
    
    // Navigation properties
    public Role Role { get; set; } = null!;
    public Site Site { get; set; } = null!;
}