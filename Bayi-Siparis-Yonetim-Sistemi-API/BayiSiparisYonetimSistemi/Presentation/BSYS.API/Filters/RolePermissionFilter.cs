using Azure;
using BSYS.Application.Abstractions.Services;
using BSYS.Application.CustomAttributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;

namespace BSYS.API.Filters;

public class RolePermissionFilter : IAsyncActionFilter
{
    readonly IUserService _userService;

    public RolePermissionFilter(IUserService userService)
    {
        _userService = userService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var name = context.HttpContext.User.Identity?.Name;
        var role = context.HttpContext.User.Identity?.AuthenticationType?.ToString();
        var id = context.HttpContext.User.Identities.FirstOrDefault();
        var authenticated = context.HttpContext.User.Identity?.IsAuthenticated;
        var connectionid = context.HttpContext.Connection.Id;

        Console.WriteLine(role);
        if (!string.IsNullOrEmpty(name) && name != "admin" && name != "Admin")
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var attribute = descriptor.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;

            var httpAttribute = descriptor.MethodInfo.GetCustomAttribute(typeof(HttpMethodAttribute)) as HttpMethodAttribute;

            var code = $"{(httpAttribute != null ? httpAttribute.HttpMethods.First() : HttpMethods.Get)}.{attribute.ActionType}.{attribute.Definition.Replace(" ", "")}";

            var hasRole = await _userService.HasRolePermissionToEndpointAsync(name, code);

            if (!hasRole)
                context.Result = new UnauthorizedResult();
            else
                await next();
        }
        else
            await next();
    }
}
