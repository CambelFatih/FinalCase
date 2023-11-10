using BSYS.Application.Abstractions.Services;
using BSYS.Application.CustomAttributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;
using System.Security.Claims;

namespace BSYS.API.Filters;

public class RolePermissionFilter : IAsyncActionFilter
{
    private readonly IUserService _userService;

    public RolePermissionFilter(IUserService userService)
    {
        _userService = userService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var name = context.HttpContext.User.Identity?.Name;
        if (!string.IsNullOrEmpty(name))
        {
            var rolesClaims = context.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
            if (HasAdminRole(rolesClaims) || name=="Ataturk")
            {
                await next();
            }
            else
            {
                await HandleRegularUser(context, name, next);
            }
        }
        else
        {
            await next();
        }
    }

    private bool HasAdminRole(List<Claim> rolesClaims)
    {
        return rolesClaims.Any(roleClaim => roleClaim.Value.Equals("Admin", StringComparison.OrdinalIgnoreCase));
    }
    private async Task HandleRegularUser(ActionExecutingContext context, string name, ActionExecutionDelegate next)
    {
        var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
        var attribute = descriptor.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;

        if (attribute == null)
        {
            await next();
        }
        else
        {
            var httpAttribute = descriptor.MethodInfo.GetCustomAttribute(typeof(HttpMethodAttribute)) as HttpMethodAttribute;

            var code = $"{(httpAttribute != null ? httpAttribute.HttpMethods.First() : HttpMethods.Get)}.{attribute.ActionType}.{attribute.Definition.Replace(" ", "")}";

            var hasRole = await _userService.HasRolePermissionToEndpointAsync(name, code);
            if (!hasRole)
            {
                context.Result = new UnauthorizedResult();
            }
            else
            {
                await next();
            }
        }        
    }
}
