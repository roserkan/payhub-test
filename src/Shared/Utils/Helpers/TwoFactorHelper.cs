using OtpNet;

namespace Shared.Utils.Helpers;

public static class TwoFactorHelper
{
    public static string GenerateSecretKey()
    {
        var key = KeyGeneration.GenerateRandomKey(20);
        return Base32Encoding.ToString(key);
    }
    
    public static bool VerifyTwoFactorCode(string secretKey, string code)
    {
        var otp = new Totp(Base32Encoding.ToBytes(secretKey));
        return otp.VerifyTotp(code, out _, new VerificationWindow(previous: 1, future: 1));
    }

    public static string GenerateQrCodeUri(string email, string secretKey)
    {
        return $"otpauth://totp/{email}?secret={secretKey}&issuer=YourAppName";
    }
}