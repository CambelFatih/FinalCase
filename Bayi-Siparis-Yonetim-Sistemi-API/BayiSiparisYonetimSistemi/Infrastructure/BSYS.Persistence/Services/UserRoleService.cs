using BSYS.Application.Abstractions.Services;
using BSYS.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

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
    public async Task<List<string>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return new List<string>();
        var roles = await _userManager.GetRolesAsync(user);
        if (user == null)
            return new List<string>();
        return roles.ToList();
    }
}
