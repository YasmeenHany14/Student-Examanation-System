using Application.DTOs;
using Shared.ResourceParameters;

namespace Application.Contracts;

public interface INotificationsClient
{
    Task ReceiveNotification(string message);
    Task LoadNotifications(PagedList<NotificationAppDto> notifications);
    Task MarkNotificationAsRead(int notificationId);
}

/*
 * 1- send notifications to ALL ADMINS on exam start
 * 2- send notifications to ALL ADMINS on exam evaluated
 * 3- send notifications to Student on his exam evaluated
 */

 /*
 * 
 */