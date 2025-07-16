using Domain.UserContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Helpers.Filters;

public class CanAccessResourceFilter(
    IUserContext userContext
    ) : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var isAuthenticated = userContext.IsAuthenticated;
        
        if (!isAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        var userId = userContext.UserId.ToString();
        var isAdmin = userContext.IsAdmin;

        string? id = null;
        var hasOwnerId = context.ActionArguments.ContainsKey("id");
        if (hasOwnerId)
            id = context.ActionArguments["id"]?.ToString() ?? string.Empty;
        else
        {
            if (context.ActionArguments.TryGetValue("id", out var objId))
                id = objId?.ToString() ?? string.Empty;
        }

        if (id != null)
        {
            if (userId != id && !isAdmin)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
        // allow the movement of the request to the next middleware only if not short-circuited
        await next();
    }
}
