namespace Application.Contracts;

public interface INotificationsService
{
    Task NotifyExamStartedAsync(int subjectId, string userId);
    Task NotifyExamEvaluatedAsync(int subjectId, int studentId, int totalScore);
}
