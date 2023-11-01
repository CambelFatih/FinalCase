using BSYS.Domain.Entities.Identity;
using BSYS.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

public class RoleMiddleware : IMiddleware
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ActiveAdiminService _activeAdminService;

    public RoleMiddleware(UserManager<AppUser> userManager, ActiveAdiminService activeAdminService)
    {
        _userManager = userManager;
        _activeAdminService = activeAdminService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId != null)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                // Kullanıcı admin rolüne sahipse, onu ActiveAdminService'e ekleyin.
                if (userRoles.Contains("Admin"))
                {
                    _activeAdminService.AddActiveAdmin(context.Connection.Id, userId);
                }
            }
        }
        else
        {
            // Kullanıcı çıkış yapmışsa, ActiveAdminService'den çıkarın.
            _activeAdminService.RemoveActiveAdmin(context.Connection.Id);
        }

        await next(context);
    }
}
