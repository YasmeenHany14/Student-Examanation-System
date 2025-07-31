using System.Security.Claims;
using Application.Contracts;
using Domain.UserContext;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs;

public class NotificationsHub(IUserContext userContext) : Hub<INotificationsClient>
{
    private readonly string _adminGroupName = "Admins";
    
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine("User is authenticated: " + userContext.IsAuthenticated);
        if (userContext.IsAuthenticated && userContext.IsAdmin)
            await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
    }
}
