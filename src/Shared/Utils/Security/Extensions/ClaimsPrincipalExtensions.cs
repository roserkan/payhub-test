using System.Security.Claims;

namespace Shared.Utils.Security.Extensions;

/// <summary>
/// Provides extension methods for <see cref="ClaimsPrincipal"/>.
/// </summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Retrieves all claims of the specified type from the <see cref="ClaimsPrincipal"/>.
    /// </summary>
    /// <param name="claimsPrincipal">The claims principal.</param>
    /// <param name="claimType">The type of claim to retrieve.</param>
    /// <returns>A list of claim values, or null if no claims are found.</returns>
    public static List<string>? Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        var result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();
        return result;
    }

    /// <summary>
    /// Retrieves all role claims from the <see cref="ClaimsPrincipal"/>.
    /// </summary>
    /// <param name="claimsPrincipal">The claims principal.</param>
    /// <returns>A list of role claim values, or null if no role claims are found.</returns>
    public static List<string>? ClaimRoles(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal?.Claims(ClaimTypes.Role);

    /// <summary>
    /// Retrieves the user ID claim from the <see cref="ClaimsPrincipal"/>.
    /// </summary>
    /// <param name="claimsPrincipal">The claims principal.</param>
    /// <returns>The user ID as an integer, or null if the user ID claim is not found.</returns>
    public static int? GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var id = claimsPrincipal?.Claims(ClaimTypes.NameIdentifier)?.FirstOrDefault();
        return id != null ? int.Parse(id) : null;
    }
    
    public static string? GetUserRole(this ClaimsPrincipal claimsPrincipal)
    {
        var role = claimsPrincipal?.Claims(ClaimTypes.Role)?.FirstOrDefault();
        return role;
    }
}