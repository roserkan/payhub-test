using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Shared.Utils.Security.Encryption;

/// <summary>
/// Provides helper methods for creating security keys.
/// </summary>
public static class SecurityKeyHelper
{
    /// <summary>
    /// Creates a symmetric security key from the specified string.
    /// </summary>
    /// <param name="securityKey">The security key as a string.</param>
    /// <returns>A symmetric security key.</returns>
    public static SecurityKey CreateSecurityKey(string securityKey) => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
}