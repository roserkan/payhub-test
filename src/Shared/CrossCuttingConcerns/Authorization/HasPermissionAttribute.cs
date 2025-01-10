using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Shared.CrossCuttingConcerns.Authorization;

public class HasPermissionAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    private readonly string[] _permissions;

    // Multiple permissions are passed as an array
    public HasPermissionAttribute(params string[] permissions)
    {
        _permissions = permissions;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        //Admin check, bypass permission checks
        if (user.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Role)?.Value == "Admin")
        {
            return;
        }

        // Get all permissions from claims
        var userPermissions = user.Claims
            .Where(c => c.Type == "permission")
            .Select(c => c.Value)
            .ToList();

        // Ensure the user has all the required permissions
        var hasAllRequiredPermissions = _permissions.All(permission => userPermissions.Contains(permission));

        if (!hasAllRequiredPermissions)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        await Task.CompletedTask;

        await Task.CompletedTask;
    }
}