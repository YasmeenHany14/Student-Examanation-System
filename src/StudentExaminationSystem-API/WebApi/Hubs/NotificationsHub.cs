using Application.Contracts;
using Domain.UserContext;
using Microsoft.AspNetCore.SignalR;
using Shared.ResourceParameters;
using WebApi.Services;

namespace WebApi.Hubs;

public class NotificationsHub(IUserContext userContext,
    INotificationsService notificationsService,
    IConnectionHelper connectionHelper
    ) : Hub<INotificationsClient>
{
    private readonly string _adminGroupName = "Admins";
    private readonly string _studentGroupName = "Students";
    
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine("User is authenticated: " + userContext.IsAuthenticated);
        if (userContext.IsAuthenticated)
        {
            if (userContext.IsAdmin)
                connectionHelper.AddUserToGroup(_adminGroupName, userContext.UserId.ToString());
            else
                connectionHelper.AddUserToGroup(_studentGroupName, userContext.UserId.ToString());
        }
        await base.OnConnectedAsync();
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine("User disconnected: " + userContext.UserId);
        if (userContext.IsAuthenticated)
        {
            if (userContext.IsAdmin)
                connectionHelper.RemoveUserFromGroup(_adminGroupName, userContext.UserId.ToString());
            else
                connectionHelper.RemoveUserFromGroup(_studentGroupName, userContext.UserId.ToString());
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task LoadNotificationsAsync(NotificationsResourceParameters resourceParameters)
    {
        if (!userContext.IsAuthenticated)
            return;

        var userId = userContext.UserId.ToString();
        Console.WriteLine("Loading notifications for user: " + userId);
        
        var notifications = await notificationsService.GetAllNotificationsAsync(resourceParameters, userId);
        
        await Clients.Caller.NotificationsLoaded(notifications.Value);
    }
    
    public async Task MarkNotificationAsReadAsync()
    {
        if (!userContext.IsAuthenticated)
            return;
        

        var userId = userContext.UserId.ToString();
        Console.WriteLine($"Marking notifications as read for user: {userId}");
        var result = await notificationsService.MarkAllNotificationsAsReadAsync(userId);
    }
    
}
