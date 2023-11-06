using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSYS.Application.Abstractions.Hubs;

public interface IAdminHubService
{
    public bool IsAdminActive(string userId);
    public void AddActiveAdmin(string connectionId, string userId);
    public void RemoveInactiveAdmin(string connectionId);
    public bool IsUserActive(string userId);
    public void RemoveInactiveUser(string connectionId);
    public void AddActiveUser(string connectionId, string userId);
    public string AssignAdminToCustomer(string customerId);
    public void DisconnectCustomerFromAdmin(string customerId);
    public string GetCustomerConnectionId(string customerId);
}
