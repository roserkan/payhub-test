using System.Security.Cryptography;
using System.Text;

namespace Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Helpers;

public static class SignatureHelper
{
    public static string CreateSignature(Dictionary<string, string>? requestData, string apiSecret)
    {
        if (requestData is null)
        {
            using (var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(apiSecret)))
            {
                var hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(String.Empty));
                return Convert.ToBase64String(hash);
            }
        }
        
        var encodedRequest = new FormUrlEncodedContent(requestData!).ReadAsStringAsync().Result;
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(apiSecret)))
        {
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(encodedRequest));
            return Convert.ToBase64String(hash);
        }
    }
}