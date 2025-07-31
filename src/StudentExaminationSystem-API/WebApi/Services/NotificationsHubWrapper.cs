using Application.Contracts;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace WebApi.Services;

public class NotificationsHubWrapper(IHubContext<NotificationsHub, INotificationsClient> hubContext) : INotificationsHub
{
    public async Task SendEvaluationCompletedAsync(string userId, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return;

        Console.WriteLine("Sending evaluation completed notification to user: " + userId);
        
        if (userId == "Admins")
            await hubContext.Clients.Group("Admins").ReceiveNotification(message);
        else
            await hubContext.Clients.User(userId).ReceiveNotification(message);
    }

    public async Task SendExamStartedAsync(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return;
        
        Console.WriteLine("Sending exam started notification: " + message);

        await hubContext.Clients.Group("Admins").ReceiveNotification(message);
    }
}
