using Application.DTOs;
using Shared.ResourceParameters;

namespace Application.Contracts;

public interface INotificationsClient
{
    Task ReceiveNotification(NotificationAppDto notification);
    Task NotificationsLoaded(PagedList<NotificationAppDto> notifications);
    Task MarkNotificationAsReadAsync();
}
