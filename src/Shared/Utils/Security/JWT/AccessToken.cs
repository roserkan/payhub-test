namespace Shared.Utils.Security.JWT;

/// <summary>
/// Represents an access token with a token string and expiration date.
/// </summary>
public class AccessToken
{
    /// <summary>
    /// Gets or sets the token string.
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Gets or sets the expiration date of the token.
    /// </summary>
    public DateTime Expiration { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessToken"/> class.
    /// </summary>
    public AccessToken()
    {
        Token = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessToken"/> class with the specified token and expiration date.
    /// </summary>
    /// <param name="token">The token string.</param>
    /// <param name="expiration">The expiration date of the token.</param>
    public AccessToken(string token, DateTime expiration)
    {
        Token = token;
        Expiration = expiration;
    }
}