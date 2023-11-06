using BSYS.SignalR.Hubs;
using Microsoft.AspNetCore.Builder;

namespace BSYS.SignalR;

public static class HubRegistration
{
    public static void MapHubs(this WebApplication webApplication)
    {
        webApplication.MapHub<ProductHub>("/products-hub");
        webApplication.MapHub<OrderHub>("/orders-hub");
        webApplication.MapHub<AdminHub>("/admins-hub");
    }
}
