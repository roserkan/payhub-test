using Shared.Domain;

namespace Payhub.Domain.Entities.UserManagement;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = new byte[] { };
    public byte[] PasswordSalt { get; set; } = new byte[] { };
    public bool IsTwoFactorEnabled { get; set; }
    public string? TwoFactorSecret { get; set; }
    public bool IsDeleted { get; set; }
    public string? FirstPassword { get; set; }
    
    // Navigation properties
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}