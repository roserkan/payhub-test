namespace Payhub.Application.Common.DTOs.Roles;

public sealed record SitePermissionDto
{
    public int Id { get; set; }
    public string SiteName { get; set; } = string.Empty;
}