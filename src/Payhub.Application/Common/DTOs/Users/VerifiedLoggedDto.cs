using System.Text.Json.Serialization;
using Payhub.Domain.Enums;
using Shared.Utils.Security.JWT;

namespace Payhub.Application.Common.DTOs.Users;

public sealed record VerifiedLoggedDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Username { get; set; } = null!;
    
    public List<string>? Roles { get; set; }
    public RoleType RoleType { get; set; }
    public List<string>? SystemPermissions { get; set; }
    public List<int>? SiteIds { get; set; }
    public List<int>? AffiliateIds { get; set; }
    public bool IsTwoFactorEnabled { get; set; }
    public AccessToken AccessToken { get; set; } = null!;
}