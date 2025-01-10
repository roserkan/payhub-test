using Payhub.Domain.Enums;

namespace Payhub.Application.Common.DTOs.Users;

public class CurrentUserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public List<string>? Roles { get; set; }
    public RoleType? RoleType { get; set; }
    public List<string>? Permissions { get; set; }
    public List<int>? Affiliates { get; set; }
}