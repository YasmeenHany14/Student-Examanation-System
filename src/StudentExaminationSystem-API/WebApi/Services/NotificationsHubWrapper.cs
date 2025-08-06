using Application.Contracts;
using Application.DTOs;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace WebApi.Services;

public class NotificationsHubWrapper(
    IHubContext<NotificationsHub, 
    INotificationsClient> hubContext,
    IConnectionHelper connectionHelper
    ) : INotificationsHub
{
    private readonly string _adminGroupName = "Admins";
    private readonly string _studentGroupName = "Students";
    public async Task SendEvaluationCompletedAsync(string userId, NotificationAppDto notification)
    {
        if (connectionHelper.CheckUserInGroup(_studentGroupName, userId))
            await hubContext.Clients.User(userId).ReceiveNotification(notification);
    }
    
    public async Task SendAdminNotificationsAsync(Dictionary<string, NotificationAppDto> notifications)
    {
        foreach (var notification in notifications)
        {
            if (connectionHelper.CheckUserInGroup(_adminGroupName, notification.Key))
                await hubContext.Clients.User(notification.Key).ReceiveNotification(notification.Value);
        }
    }
}
