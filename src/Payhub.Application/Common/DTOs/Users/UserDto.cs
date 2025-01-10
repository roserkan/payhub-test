namespace Payhub.Application.Common.DTOs.Users;

public sealed record UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public string? TwoFactorSecret { get; set; }
    public bool IsTwoFactorEnabled { get; set; }
    public string? FirstPassword { get; set; }
}