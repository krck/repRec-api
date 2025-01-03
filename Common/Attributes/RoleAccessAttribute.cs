using Microsoft.AspNetCore.Mvc.Filters;
using RepRecApi.Common.Services;
using Microsoft.AspNetCore.Mvc;
using RepRecApi.Common.Enums;

namespace RepRecApi.Common.Attributes;

public class RoleAccessAttribute : Attribute, IAsyncActionFilter
{
    private readonly EnumRoles[] _roles;

    public RoleAccessAttribute(params EnumRoles[] roles)
    {
        _roles = roles;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var userId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == GlobalStaticVariables.Auth0UserIdClaim)?.Value;
        if (userId == null)
        {
            context.Result = new UnauthorizedResult(); // Unauthorized
            return;
        }
        else
        {
            var userService = (IUserService?)context.HttpContext.RequestServices.GetService(typeof(IUserService));
            if (userService == null)
            {
                context.Result = new StatusCodeResult(500); // Internal Server Error
                return;
            }

            // Get the User Roles from the UserService and validate if the user has the required ones
            var userRoles = await userService.GetUserRolesAsync(userId);
            if (!_roles.Any(r => userRoles.Contains(r)))
            {
                context.Result = new ForbidResult(); // Forbidden
                return;
            }
        }
        await next();
    }
}
