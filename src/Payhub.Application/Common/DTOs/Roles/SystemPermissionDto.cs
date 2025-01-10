namespace Payhub.Application.Common.DTOs.Roles;

public sealed record SystemPermissionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string PermissionGroup { get; set; } = string.Empty;
    public string? Description { get; set; }
}