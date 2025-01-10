namespace Payhub.Application.Common.DTOs.Roles;

public sealed record AffiliatePermissionDto
{
    public int Id { get; set; }
    public string AffiliateName { get; set; } = string.Empty;
}