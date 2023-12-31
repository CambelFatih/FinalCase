﻿using BSYS.SignalR.Hubs;
using Microsoft.AspNetCore.Builder;

namespace BSYS.SignalR;

public static class HubRegistration
{
    public static void MapHubs(this WebApplication webApplication)
    {
        webApplication.MapHub<ChatHub>("/chat-hub");
    }
}
