using BSYS.Application.Abstractions.Services;
using BSYS.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace BSYS.Persistence.Services;

public class UserRoleService : IUserRoleService
{
    private readonly UserManager<AppUser> _userManager;

    public UserRoleService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> IsUserInRoleAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            // Kullanıcı bulunamazsa veya hatalıysa false döner.
            return false;
        }

        var roles = await _userManager.GetRolesAsync(user);
        if (roles == null)
        {
            // Kullanıcı'ya ait roller bulunamazsa veya hatalıysa false döner.
            return false;
        }

        return roles.Contains(roleName);
    }
}
