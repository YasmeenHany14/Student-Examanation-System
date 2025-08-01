﻿using System.Security.Claims;
using Application.Contracts;
using Application.DTOs;
using Domain.UserContext;
using Microsoft.AspNetCore.SignalR;
using Shared.ResourceParameters;

namespace WebApi.Hubs;

public class NotificationsHub(IUserContext userContext,
    INotificationsService notificationsService) : Hub<INotificationsClient>
{
    private readonly string _adminGroupName = "Admins";
    
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine("User is authenticated: " + userContext.IsAuthenticated);
        if (userContext.IsAuthenticated && userContext.IsAdmin)
            await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
        await base.OnConnectedAsync();
    }

    public async Task LoadNotificationsAsync(NotificationsResourceParameters resourceParameters)
    {
        if (!userContext.IsAuthenticated)
        {
            await Clients.Caller.ReceiveNotification("You must be logged in to receive notifications.");
            return;
        }

        var userId = userContext.UserId.ToString();
        Console.WriteLine("Loading notifications for user: " + userId);
        
        var notifications = await notificationsService.GetAllNotificationsAsync(resourceParameters, userId);
        
        await Clients.Caller.NotificationsLoaded(notifications.Value);
    }
    
    public async Task MarkNotificationAsReadAsync()
    {
        if (!userContext.IsAuthenticated)
        {
            await Clients.Caller.ReceiveNotification("You must be logged in to mark notifications as read.");
            return;
        }

        var userId = userContext.UserId.ToString();
        Console.WriteLine($"Marking notifications as read for user: {userId}");
        var result = await notificationsService.MarkAllNotificationsAsReadAsync(userId);
    }
    
}
