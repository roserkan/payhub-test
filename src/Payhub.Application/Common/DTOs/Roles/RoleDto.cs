using Payhub.Domain.Enums;

namespace Payhub.Application.Common.DTOs.Roles;

public sealed record RoleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public RoleType? RoleType { get; set; }
    public IEnumerable<SystemPermissionDto> SystemPermissions { get; set; } = new List<SystemPermissionDto>();
    public IEnumerable<SitePermissionDto> SitePermissions { get; set; } = new List<SitePermissionDto>();
    public IEnumerable<AffiliatePermissionDto> AffiliatePermissions { get; set; } = new List<AffiliatePermissionDto>();
}