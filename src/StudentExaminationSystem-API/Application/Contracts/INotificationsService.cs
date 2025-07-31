using Application.Common.ErrorAndResults;
using Application.DTOs;
using Shared.ResourceParameters;

namespace Application.Contracts;

public interface INotificationsService
{
    Task<Result<bool>> NotifyExamStartedAsync(int subjectId, string userId);
    Task<Result<bool>> NotifyExamEvaluatedAsync(int subjectId, int studentId, int totalScore);
    Task<Result<bool>> MarkNotificationAsReadAsync(int notificationId, string userId);
    
    Task<Result<PagedList<NotificationAppDto>>> GetAllNotificationsAsync(
        BaseResourceParameters resourceParameters, string userId);
}
