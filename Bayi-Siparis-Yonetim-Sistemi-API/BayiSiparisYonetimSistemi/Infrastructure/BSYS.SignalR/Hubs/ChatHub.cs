using Microsoft.AspNetCore.SignalR;
using BSYS.Application.Abstractions.Hubs;
using System.Security.Claims;
using BSYS.Domain.Base.Chat;

namespace BSYS.SignalR.Hubs;

public class ChatHub : Hub
{
    private readonly IChatHubService _adminHubService;

    public ChatHub(IChatHubService adminHubService)
    {
        _adminHubService = adminHubService;
    }

    public override async Task OnConnectedAsync()
    {
        string userId = Context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
        var rolesClaims = Context.User.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
        var name = Context.User.Identity?.Name;
        if (await _adminHubService.HasAdminRole(rolesClaims))
        {
            _adminHubService.CreateAdmin(name, Context.ConnectionId);
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var rolesClaims = Context.User.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
        var name = Context.User.Identity?.Name;
        if (await _adminHubService.HasAdminRole(rolesClaims))
        {
            _adminHubService.RemoveAdminAsync(name);
        }
        else
        {
            _adminHubService.RemoveBayiAndDecrementLoad(name);
        }
        await base.OnDisconnectedAsync(exception);
    }
    public async Task SendMessageToAdminAsync(string message)
    {
        // Check if the user is authenticated
        if (!Context.User.Identity.IsAuthenticated)
        {
            await Clients.Client(Context.ConnectionId).SendAsync(ReceiveFunctionNames.MessageFromAdmin, "Bu işlemi sadece Giriş yapan kullanıcılar gerçekleştirebilir");
            return;
        }
        var userId = Context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
        var rolesClaims = Context.User.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
        var name = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

        // Kullanıcının müşteri olup olmadığını kontrol et
        if (await _adminHubService.HasBayiRole(rolesClaims)==false)
        {
            await Clients.Client(Context.ConnectionId).SendAsync(ReceiveFunctionNames.MessageFromAdmin, "Bu işlemi sadece Bayiler gerçekleştirebilir.");
            return;
        }        
        if (await _adminHubService.HasActiveAdmin())
        {
            string adminConnectionId;
            if (await _adminHubService.DoesBayiExistInAdmins(name)) {
                adminConnectionId = await _adminHubService.GetAdminConnectionIdByBayiUserName(name);
            }
            else
            {
                adminConnectionId = await _adminHubService.AddBayiToMinLoadAdmin(name, Context.ConnectionId);
            }
                
            MessageInfo messageInfo = new MessageInfo
            {
                UserName = name,
                UserId = userId,
                ConnectionId = Context.ConnectionId,
                Message = message
            };
            await Clients.Client(adminConnectionId).SendAsync(ReceiveFunctionNames.MessageFromCustomer, messageInfo);
        }
        else
        {
            await Clients.Client(Context.ConnectionId).SendAsync(ReceiveFunctionNames.MessageFromAdmin, "Şu anda hiçbir admin aktif değil. Lütfen daha sonra tekrar deneyiniz.");
            return;
           
        }
    }
    public async Task SendMessageToCustomerAsync(string connectionId, string message)
    {
        string NameSurname = Context.User.Claims.FirstOrDefault(c => c.Type == "NameSurname")?.Value;
        message = NameSurname + " : " + message;
        await Clients.Client(connectionId).SendAsync(ReceiveFunctionNames.MessageFromAdmin, message);
    }

}
