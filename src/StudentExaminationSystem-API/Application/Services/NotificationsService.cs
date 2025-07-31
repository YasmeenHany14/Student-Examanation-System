using Application.Common.Constants.Notifications;
using Application.Contracts;
using Domain.Repositories;

namespace Application.Services;

public class NotificationsService(
    IUnitOfWork unitOfWork,
    INotificationsHub notificationsHub
    ): INotificationsService
{
    public async Task NotifyExamStartedAsync(int subjectId, string userId)
    {
        var subject = await unitOfWork.SubjectRepository.GetByIdAsync(subjectId);
        var student = await unitOfWork.StudentRepository.GetByIdAsync(userId);
        var message = string.Format(Notifications.ExamStarted, student?.Name, subject?.Name);
    }

    public async Task NotifyExamEvaluatedAsync(int subjectId, int studentId, int totalScore)
    {
        var subject = await unitOfWork.SubjectRepository.GetByIdAsync(subjectId);
        var student = await unitOfWork.StudentRepository.GetByIdAsync(studentId);
        var studentMsg = string.Format(Notifications.ExamEvaluatedStudent, subject?.Name, totalScore);
        var adminMsg = string.Format(Notifications.ExamEvaluatedAdmin, subject?.Name, totalScore,
            student.User.FirstName + " " + student.User.LastName);

        await notificationsHub.SendEvaluationCompletedAsync(student.UserId, studentMsg);
        await notificationsHub.SendExamStartedAsync(adminMsg);
    }
}
