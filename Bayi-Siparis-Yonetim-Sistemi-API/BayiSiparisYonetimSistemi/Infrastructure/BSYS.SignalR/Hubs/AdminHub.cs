﻿using Microsoft.AspNetCore.SignalR;
using BSYS.Application.Abstractions.Hubs;
using System.Security.Claims;

namespace BSYS.SignalR.Hubs;

public class AdminHub : Hub
{
    private readonly IAdminHubService _adminHubService;

    public AdminHub(IAdminHubService adminHubService)
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
        var userId = Context.UserIdentifier;
        var rolesClaims = Context.User.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();

        // Kullanıcının müşteri olup olmadığını kontrol et
        if (await HasAdminRole(rolesClaims)==false)
        {
            await Clients.Caller.SendAsync("Unauthorized", "Bu işlemi sadece müşteriler gerçekleştirebilir.");
            return;
        }
        var adminId = await _adminHubService.AssignAdminToCustomer(userId);
        if (adminId == null)
        {
            await Clients.Caller.SendAsync("NoAdminAvailable", "Şu anda hiçbir admin aktif değil. Lütfen daha sonra tekrar deneyiniz.");
        }
        else
        {
            // Müşteri ve admini aynı gruba ekle
            await Groups.AddToGroupAsync(Context.ConnectionId, adminId);
            await Clients.Group(adminId).SendAsync(ReceiveFunctionNames.MessageFromCustomer, message, userId);
        }
    }
    public async Task SendMessageToCustomerAsync(string customerId, string message)
    {
        // Adminin müşteriye mesaj göndermesini sağla
        var connectionId = await _adminHubService.GetCustomerConnectionId(customerId);
        if (connectionId != null)
        {
            await Clients.Client(connectionId).SendAsync(ReceiveFunctionNames.MessageFromAdmin, message);
        }
        else
        {
            await Clients.Caller.SendAsync("CustomerNotAvailable", "Bu müşteri şu anda aktif değil.");
        }
    }

    private async Task<bool> HasAdminRole(List<Claim> rolesClaims)
    {
        return rolesClaims.Any(roleClaim => roleClaim.Value.Equals("Admin", StringComparison.OrdinalIgnoreCase));
    }
}
