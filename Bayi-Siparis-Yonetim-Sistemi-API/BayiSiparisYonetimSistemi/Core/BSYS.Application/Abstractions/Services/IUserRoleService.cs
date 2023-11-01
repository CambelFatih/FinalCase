namespace BSYS.Application.Abstractions.Services;

public interface IUserRoleService
{
    Task<bool> IsUserInRoleAsync(string userId, string roleName);
}
