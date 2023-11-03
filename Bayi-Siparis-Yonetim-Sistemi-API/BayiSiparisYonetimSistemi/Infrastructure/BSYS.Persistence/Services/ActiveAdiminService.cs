namespace BSYS.Persistence.Services
{
    //signalR için oluşturulmuş servis
    public class ActiveAdminService
    {
        private readonly Dictionary<string, string> activeAdmins = new Dictionary<string, string>();

        public void AddActiveAdmin(string connectionId, string userId)
        {
            activeAdmins[connectionId] = userId;
        }

        public void RemoveActiveAdmin(string connectionId)
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
}
