using Microsoft.AspNetCore.Http;

namespace Shared.Utils.Helpers;

public static class HeaderHelper
{
    public static string? GetApiKey(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("apiKey", out var headerValue))
        {
            return headerValue.ToString();
        }
        return null;
    }
    
    public static string? GetSecurityKey(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("securityKey", out var headerValue))
        {
            return headerValue.ToString();
        }
        return null;
    }
}