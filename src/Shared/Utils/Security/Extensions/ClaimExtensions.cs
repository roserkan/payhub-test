using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Shared.Utils.Security.Extensions;

/// <summary>
/// Provides extension methods for adding claims.
/// </summary>
public static class ClaimExtensions
{
    /// <summary>
    /// Adds an email claim to the specified claims collection.
    /// </summary>
    /// <param name="claims">The claims collection.</param>
    /// <param name="email">The email to add as a claim.</param>
    public static void AddEmail(this ICollection<Claim> claims, string email) =>
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));

    /// <summary>
    /// Adds a name claim to the specified claims collection.
    /// </summary>
    /// <param name="claims">The claims collection.</param>
    /// <param name="name">The name to add as a claim.</param>
    public static void AddName(this ICollection<Claim> claims, string name) => claims.Add(new Claim(ClaimTypes.Name, name));

    /// <summary>
    /// Adds a name identifier claim to the specified claims collection.
    /// </summary>
    /// <param name="claims">The claims collection.</param>
    /// <param name="nameIdentifier">The name identifier to add as a claim.</param>
    public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier) =>
        claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));

    /// <summary>
    /// Adds role claims to the specified claims collection.
    /// </summary>
    /// <param name="claims">The claims collection.</param>
    /// <param name="roles">The roles to add as claims.</param>
    public static void AddRoles(this ICollection<Claim> claims, string[] roles) =>
        roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
}