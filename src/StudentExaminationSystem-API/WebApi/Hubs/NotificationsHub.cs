using System.Security.Claims;
using Application.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs;

public class NotificationsHub : Hub<INotificationsClient>, INotificationsHub
{
    private readonly string _adminGroupName = "Admins";
    public override async Task OnConnectedAsync()
    {
        var isAdmin = Context.User?.Claims.Any(c =>
            c.Type == ClaimTypes.Role && c.Value == "Admin") ?? false;
        
        if (isAdmin)
            await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");

    }
    
    public async Task SendEvaluationCompletedAsync(string userId, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return;

        await Clients.Groups(_adminGroupName).ReceiveNotification(message);
        await Clients.User(userId).ReceiveNotification(message);
    }

    public async Task SendExamStartedAsync(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return;

        await Clients.Groups(_adminGroupName).ReceiveNotification(message);
    }
}
