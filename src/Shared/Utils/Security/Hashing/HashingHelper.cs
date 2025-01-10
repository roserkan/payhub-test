using System.Security.Cryptography;
using System.Text;

namespace Shared.Utils.Security.Hashing;

/// <summary>
/// Provides helper methods for hashing passwords and values.
/// </summary>
public static class HashingHelper
{
    /// <summary>
    /// Creates a password hash and salt for the specified password.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <param name="passwordHash">The resulting password hash.</param>
    /// <param name="passwordSalt">The resulting password salt.</param>
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using HMACSHA512 hmac = new();

        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    /// <summary>
    /// Verifies the specified password against the provided hash and salt.
    /// </summary>
    /// <param name="password">The password to verify.</param>
    /// <param name="passwordHash">The hash of the password.</param>
    /// <param name="passwordSalt">The salt of the password.</param>
    /// <returns><c>true</c> if the password is verified; otherwise, <c>false</c>.</returns>
    public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using HMACSHA512 hmac = new(passwordSalt);

        byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return computedHash.SequenceEqual(passwordHash);
    }
    
    public static string Sha256Hash(string value)
    {
        var messageBytes = Encoding.UTF8.GetBytes(value);
        var hashedBytes = SHA256.HashData(messageBytes);
        var hashedKey = BitConverter.ToString(hashedBytes).Replace("-", string.Empty);
        return hashedKey.ToLower();
    }
}