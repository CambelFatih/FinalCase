using BSYS.Application.Abstractions.Services;
using BSYS.Application.CustomAttributes;
using BSYS.Persistence.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;
using System.Security.Claims;

namespace BSYS.API.Filters
{
    public class RolePermissionFilter : IAsyncActionFilter
    {
        private readonly IUserService _userService;
        private readonly ActiveAdminService _activeAdminService;

        public RolePermissionFilter(IUserService userService, ActiveAdminService activeAdminService)
        {
            _userService = userService;
            _activeAdminService = activeAdminService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var name = context.HttpContext.User.Identity?.Name;
            var rolesClaims = context.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();

            if (!string.IsNullOrEmpty(name))
            {
                if (HasAdminRole(rolesClaims))
                {
                    await HandleAdminUser(context, next);
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

        private async Task HandleAdminUser(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userIdClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id");
            string userId = userIdClaim?.Value;
            _activeAdminService.AddActiveAdmin(context.HttpContext.Connection.Id, userId);
            await next();
        }

        private async Task HandleRegularUser(ActionExecutingContext context, string name, ActionExecutionDelegate next)
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var attribute = descriptor.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;

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
