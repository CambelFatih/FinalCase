using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSYS.Application.Abstractions.Hubs;

public interface IChatHubService
{
    void SetAdminConnectionIdFromProperty(string id);
    Task<string> GetAdminConnectionIdFromProperty();
    Task<string> GetAdminConnectionId();
    Task<string> AssignAdminToCustomer(string customerId);
    void AddActiveUser(string connectionId, string userId);
    void RemoveInactiveUser(string connectionId);
    Task<bool> IsUserActive(string userId);
    void AddActiveAdmin(string connectionId, string userId);
    void RemoveInactiveAdmin(string connectionId);
    Task<bool> IsAdminActive(string userId);
    Task<string> GetCustomerConnectionId(string customerId);
    void DisconnectCustomerFromAdmin(string customerId);
}
