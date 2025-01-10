using Microsoft.IdentityModel.Tokens;

namespace Shared.Utils.Security.Encryption;

/// <summary>
/// Provides helper methods for creating signing credentials.
/// </summary>
public static class SigningCredentialsHelper
{
    /// <summary>
    /// Creates signing credentials using the specified security key.
    /// </summary>
    /// <param name="securityKey">The security key.</param>
    /// <returns>Signing credentials.</returns>
    public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey) =>
        new(securityKey, SecurityAlgorithms.HmacSha512Signature);
}