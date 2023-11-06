using BSYS.Application.Abstractions.Services;

namespace BSYS.Persistence.Services;

//signalR için oluşturulmuş servis
public class ActiveAdminService : IActiveAdminService
{
    private readonly Dictionary<string, string> activeAdmins = new Dictionary<string, string>();

    public void AddActiveAdmin(string connectionId, string userId)
    {
        activeAdmins[connectionId] = userId;
    }

    public void RemoveInactiveAdmin(string connectionId)
    {
        if (activeAdmins.ContainsKey(connectionId))
        {
            activeAdmins.Remove(connectionId);
        }
    }

    public bool IsAdminActive(string userId)
    {
        return activeAdmins.ContainsValue(userId);
    }
}
