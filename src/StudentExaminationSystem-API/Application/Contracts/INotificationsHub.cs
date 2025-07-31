namespace Application.Contracts;

public interface INotificationsHub
{
    Task SendEvaluationCompletedAsync(string userId, string message);
    Task SendExamStartedAsync(string message);
}
