using Payhub.Domain.Entities.AffiliateManagement;
using Shared.Domain;

namespace Payhub.Domain.Entities.UserManagement;

public class RoleAffiliatePermission: BaseEntity
{
    public int RoleId { get; set; }
    public int AffiliateId { get; set; }
    
    // Navigation properties
    public Role Role { get; set; } = null!;
    public Affiliate Affiliate { get; set; } = null!;
}