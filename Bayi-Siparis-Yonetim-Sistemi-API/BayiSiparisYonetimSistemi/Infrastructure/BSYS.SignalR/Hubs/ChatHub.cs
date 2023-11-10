using Microsoft.AspNetCore.SignalR;
using BSYS.Application.Abstractions.Hubs;
using System.Security.Claims;
using BSYS.Domain.Base.Chat;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        if (await HasAdminRole(rolesClaims))
        {
            _adminHubService.AddActiveAdmin(Context.ConnectionId, userId);
            _adminHubService.SetAdminConnectionIdFromProperty(Context.ConnectionId);
        }
        else if(userId != null)
        { 
            _adminHubService.AddActiveUser(Context.ConnectionId, userId);
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var rolesClaims = Context.User.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
        if (await HasAdminRole(rolesClaims))
        {
            _adminHubService.RemoveInactiveAdmin(Context.ConnectionId);
        }
        else
            _adminHubService.RemoveInactiveUser(Context.ConnectionId);
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
        if (await HasBayiRole(rolesClaims)==false)
        {
           // await Clients.Client(Context.ConnectionId).SendAsync(ReceiveFunctionNames.MessageFromCustomer, "Bu işlemi sadece Bayiler gerçekleştirebilir.");
            return;
        }
        //var adminConnectionId = await _adminHubService.GetAdminConnectionIdFromProperty();
        var adminConnectionId = await _adminHubService.GetAdminConnectionId();
        if (adminConnectionId == null)
        {
            await Clients.Client(Context.ConnectionId).SendAsync(ReceiveFunctionNames.MessageFromAdmin, "Şu anda hiçbir admin aktif değil. Lütfen daha sonra tekrar deneyiniz.");
            return;
        }
        else
        {
            MessageInfo messageInfo = new MessageInfo
            {
                UserName = name,
                UserId = userId,
                ConnectionId = Context.ConnectionId,
                Message = message
            };
            await Clients.Client(adminConnectionId).SendAsync(ReceiveFunctionNames.MessageFromCustomer, messageInfo);
        }
    }
    public async Task SendMessageToCustomerAsync(string connectionId, string message)
    {
        string NameSurname = Context.User.Claims.FirstOrDefault(c => c.Type == "NameSurname")?.Value;
        message = NameSurname + " : " + message;
        await Clients.Client(connectionId).SendAsync(ReceiveFunctionNames.MessageFromAdmin, message);
        // Adminin müşteriye mesaj göndermesini sağla
        //var connectionId = await _adminHubService.GetCustomerConnectionId(customerId);
        //if (connectionId != null)
        //{
        //    await Clients.Client(connectionId).SendAsync(ReceiveFunctionNames.MessageFromAdmin, message);
        //    return;
        //}
        //else
        //{
        //    await Clients.Client(connectionId).SendAsync("CustomerNotAvailable", "Bu müşteri şu anda aktif değil.");
        //}
    }
    private async Task<bool> HasBayiRole(List<Claim> rolesClaims)
    {
        return rolesClaims.Any(roleClaim => roleClaim.Value.Equals("Bayi", StringComparison.OrdinalIgnoreCase));
    }
    private async Task<bool> HasAdminRole(List<Claim> rolesClaims)
    {
        return rolesClaims.Any(roleClaim => roleClaim.Value.Equals("Admin", StringComparison.OrdinalIgnoreCase));
    }
}
