using BSYS.Application.Abstractions.Hubs;
using BSYS.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace BSYS.SignalR.HubServices;

public class ProductHubService : IProductHubService
{
    readonly IHubContext<ProductHub> _hubContext;

    public ProductHubService(IHubContext<ProductHub> hubContext)
    {
        _hubContext = hubContext;
    }
    public async Task ProductAddedMessageAsync(string message)
    => await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.ProductAddedMessage, message);
}
