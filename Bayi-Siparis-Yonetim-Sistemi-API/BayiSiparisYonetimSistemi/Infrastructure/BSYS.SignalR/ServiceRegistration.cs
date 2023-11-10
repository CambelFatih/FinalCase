using BSYS.Application.Abstractions.Hubs;
using BSYS.SignalR.HubServices;
using Microsoft.Extensions.DependencyInjection;

namespace BSYS.SignalR;

public static class ServiceRegistration
{
    public static void AddSignalRServices(this IServiceCollection collection)
    {
        collection.AddSingleton<IChatHubService, ChatHubService>();
        collection.AddSignalR();
    }
}
