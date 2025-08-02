using Application.DTOs;
using Shared.ResourceParameters;

namespace Application.Contracts;

public interface INotificationsClient
{
    Task ReceiveNotification(string message);
    Task NotificationsLoaded(PagedList<NotificationAppDto> notifications);
    Task MarkNotificationAsReadAsync();
}