namespace Shared.Utils.Security.JWT;

/// <summary>
/// Represents the options for token generation.
/// </summary>
public class TokenOptions
{
    /// <summary>
    /// Gets or sets the audience for the token.
    /// </summary>
    public string Audience { get; set; }

    /// <summary>
    /// Gets or sets the issuer of the token.
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    /// Gets or sets the expiration time (in minutes) for the access token.
    /// </summary>
    public int AccessTokenExpiration { get; set; }

    /// <summary>
    /// Gets or sets the security key used for signing the token.
    /// </summary>
    public string SecurityKey { get; set; }

    /// <summary>
    /// Gets or sets the time-to-live (in days) for the refresh token.
    /// </summary>
    public int RefreshTokenTTL { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenOptions"/> class.
    /// </summary>
    public TokenOptions()
    {
        Audience = string.Empty;
        Issuer = string.Empty;
        SecurityKey = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenOptions"/> class with the specified parameters.
    /// </summary>
    /// <param name="audience">The audience for the token.</param>
    /// <param name="issuer">The issuer of the token.</param>
    /// <param name="accessTokenExpiration">The expiration time (in minutes) for the access token.</param>
    /// <param name="securityKey">The security key used for signing the token.</param>
    /// <param name="refreshTokenTtl">The time-to-live (in days) for the refresh token.</param>
    public TokenOptions(string audience, string issuer, int accessTokenExpiration, string securityKey, int refreshTokenTtl)
    {
        Audience = audience;
        Issuer = issuer;
        AccessTokenExpiration = accessTokenExpiration;
        SecurityKey = securityKey;
        RefreshTokenTTL = refreshTokenTtl;
    }
}

