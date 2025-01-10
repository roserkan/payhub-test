namespace Shared.Utils.Security.JWT;

/// <summary>
/// Defines methods for creating access tokens.
/// </summary>
public interface ITokenHelper
{
    AccessToken CreateToken(int id, string[]? roles, string[]? permissions, int[]? siteIds, int[]? affiliateIds, int roleType);
}