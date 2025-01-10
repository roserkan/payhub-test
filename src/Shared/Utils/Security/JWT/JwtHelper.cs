using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Utils.Security.Encryption;

namespace Shared.Utils.Security.JWT;

/// <summary>
/// Provides methods for creating JWT tokens.
/// </summary>
public class JwtHelper : ITokenHelper
{
    /// <summary>
    /// Gets the configuration.
    /// </summary>
    public IConfiguration Configuration { get; }

    private readonly TokenOptions _tokenOptions;
    private DateTime _accessTokenExpiration;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtHelper"/> class.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <exception cref="NullReferenceException">Thrown when the "TokenOptions" section is not found in the configuration.</exception>
    public JwtHelper(IConfiguration configuration)
    {
        Configuration = configuration;
        const string configurationSection = "TokenOptions";
        _tokenOptions =
            Configuration.GetSection(configurationSection).Get<TokenOptions>()
            ?? throw new NullReferenceException($"\"{configurationSection}\" section cannot found in configuration.");
    }

    public AccessToken CreateToken(int id, string[]? roles, string[]? permissions, int[]? siteIds, int[]? affiliateIds, int roleType)
    {
        _accessTokenExpiration = DateTime.Now.AddHours(12);
        SecurityKey securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
        SigningCredentials signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
        JwtSecurityToken jwt = CreateJwtSecurityToken(_tokenOptions, id, roles, permissions, siteIds, affiliateIds, roleType, signingCredentials);
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        string? token = jwtSecurityTokenHandler.WriteToken(jwt);

        return new AccessToken { Token = token, Expiration = _accessTokenExpiration };
    }

    public JwtSecurityToken CreateJwtSecurityToken(
        TokenOptions tokenOptions,
        int id,
        string[]? roles, string[]? permissions, int[]? siteIds, int[]? affiliateIds, int roleType,
        SigningCredentials signingCredentials
    )
    {
        JwtSecurityToken jwt =
            new(
                tokenOptions.Issuer,
                tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(id, roles, permissions, siteIds, affiliateIds, roleType),
                signingCredentials: signingCredentials
            );
        return jwt;
    }

    private IEnumerable<Claim> SetClaims(int id, string[]? roles, string[]? permissions, int[]? siteIds, int[]? affiliateIds, int roleType)
    {
        List<Claim> claims = new();

        // User identifier
        claims.Add(new Claim(ClaimTypes.NameIdentifier, id.ToString()));

        // Roles
        if (roles != null)
        {
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }
        
        claims.Add(new Claim("roleType", roleType.ToString()));

        // Permissions
        if (permissions != null)
        {
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("permission", permission)); // Custom claim type
            }
        }

        // Site IDs
        if (siteIds != null)
        {
            foreach (var siteId in siteIds)
            {
                claims.Add(new Claim("siteId", siteId.ToString())); // Custom claim type
            }
        }

        // Affiliate IDs
        if (affiliateIds != null)
        {
            foreach (var affiliateId in affiliateIds)
            {
                claims.Add(new Claim("affiliateId", affiliateId.ToString())); // Custom claim type
            }
        }

        return claims;
    }

}