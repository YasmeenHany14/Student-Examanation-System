using Application.DTOs;

namespace Application.Contracts;

public interface INotificationsHub
{
    Task SendEvaluationCompletedAsync(string userId, NotificationAppDto notification);
    Task SendAdminNotificationsAsync(Dictionary<string, NotificationAppDto> notifications);
}
