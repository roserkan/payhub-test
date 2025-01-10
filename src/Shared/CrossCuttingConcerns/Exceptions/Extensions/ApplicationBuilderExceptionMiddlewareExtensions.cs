using Microsoft.AspNetCore.Builder;
using Shared.CrossCuttingConcerns.Exceptions.Middleware;

namespace Shared.CrossCuttingConcerns.Exceptions.Extensions;

public static class ApplicationBuilderExceptionMiddlewareExtensions
{
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}