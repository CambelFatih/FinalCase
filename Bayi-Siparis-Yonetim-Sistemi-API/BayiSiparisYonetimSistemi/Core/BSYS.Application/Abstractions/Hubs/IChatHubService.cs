using System.Security.Claims;

namespace BSYS.Application.Abstractions.Hubs;

public interface IChatHubService
{
    Task<string> GetAdminConnectionIdByBayiUserName(string bayiUserName);
    void RemoveAdminAsync(string userName);
    void RemoveBayiAndDecrementLoad(string bayiUserName);
    Task<bool> DoesBayiExistInAdmins(string bayiUserName);
    void CreateAdmin(string userName, string connectionId);
    Task<string> AddBayiToMinLoadAdmin(string bayiUserName, string bayiConnectionId);
    Task<bool> HasActiveAdmin();
    Task<bool> HasBayiRole(List<Claim> rolesClaims);
    Task<bool> HasAdminRole(List<Claim> rolesClaims);
}
