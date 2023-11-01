using BSYS.Application.Abstractions.Hubs;
using BSYS.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace BSYS.SignalR.HubServices;

public class OrderHubService : IOrderHubService
{
    readonly IHubContext<OrderHub> _hubContext;

    public OrderHubService(IHubContext<OrderHub> hubContext)
    {
        _hubContext = hubContext;
    }
    
    public async Task OrderAddedMessageAsync(string message)
        => await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.OrderAddedMessage, message);
}
