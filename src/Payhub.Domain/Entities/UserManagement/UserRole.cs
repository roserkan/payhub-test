using Shared.Domain;

namespace Payhub.Domain.Entities.UserManagement;

public class UserRole: BaseEntity
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
    
    // Navigation properties
    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;
}