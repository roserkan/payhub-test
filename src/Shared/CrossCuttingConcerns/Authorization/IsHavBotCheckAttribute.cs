using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Shared.CrossCuttingConcerns.Authorization;

public class IsHavBotCheckAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        //Admin check, bypass permission checks
        if (user.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Role)?.Value == "HavBot")
        {
            return;
        }
        await Task.CompletedTask;
    }
}