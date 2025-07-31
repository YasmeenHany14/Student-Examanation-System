using Application.Common.Constants.Errors;
using Application.Common.Constants.Notifications;
using Application.Common.ErrorAndResults;
using Application.Contracts;
using Application.DTOs;
using Application.Mappers;
using Domain.Models;
using Domain.Repositories;
using Shared.ResourceParameters;

namespace Application.Services;

public class NotificationsService(
    IUnitOfWork unitOfWork,
    INotificationsHub notificationsHub
    ): INotificationsService
{
    public async Task<Result<bool>> NotifyExamStartedAsync(int subjectId, string userId)
    {
        var subject = await unitOfWork.SubjectRepository.GetByIdAsync(subjectId);
        var student = await unitOfWork.StudentRepository.GetByIdAsync(userId);
        var message = string.Format(Notifications.ExamStarted, student?.Name, subject?.Name);
        
        await unitOfWork.NotificationsRepository.CreateAdminNotificationAsync(message);
        var result = await unitOfWork.SaveChangesAsync();
        
        if (result <= 0)
            return Result<bool>.Failure(CommonErrors.InternalServerError());
        
        await notificationsHub.SendExamStartedAsync(message);
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> NotifyExamEvaluatedAsync(int subjectId, int studentId, int totalScore)
    {
        var subject = await unitOfWork.SubjectRepository.GetByIdAsync(subjectId);
        var student = await unitOfWork.StudentRepository.GetByIdAsync(studentId);
        var studentMsg = string.Format(Notifications.ExamEvaluatedStudent, subject?.Name, totalScore);
        var adminMsg = string.Format(Notifications.ExamEvaluatedAdmin, subject?.Name, totalScore,
            student!.User!.FirstName + " " + student.User.LastName);
        
        var studentNotification = new Notification(student.UserId, studentMsg);
        
        await unitOfWork.NotificationsRepository.AddAsync(studentNotification);
        await unitOfWork.NotificationsRepository.CreateAdminNotificationAsync(adminMsg);
        
        var result = await unitOfWork.SaveChangesAsync();
        if (result <= 0)
            return Result<bool>.Failure(CommonErrors.InternalServerError());

        await notificationsHub.SendEvaluationCompletedAsync(student.UserId, studentMsg);
        await notificationsHub.SendEvaluationCompletedAsync("Admins", adminMsg);
        
        return Result<bool>.Success(true);
    }
    
    public async Task<Result<bool>> MarkNotificationAsReadAsync(int notificationId, string userId)
    {
        var notification = await unitOfWork.NotificationsRepository.GetByIdAsync(notificationId);
        if (notification == null || notification.UserId != userId)
            return Result<bool>.Failure(CommonErrors.NotFound());

        notification.IsRead = true;
        unitOfWork.NotificationsRepository.UpdateAsync(notification);
        
        var result = await unitOfWork.SaveChangesAsync();
        if (result <= 0)
            return Result<bool>.Failure(CommonErrors.InternalServerError());

        return Result<bool>.Success(true);
    }

    public async Task<Result<PagedList<NotificationAppDto>>> GetAllNotificationsAsync(BaseResourceParameters resourceParameters, string userId)
    {
        var notifications = await unitOfWork.NotificationsRepository.GetNotificationsByUserIdAsync(resourceParameters, userId);
        var notificationDtos = notifications.Data.Select(n => n.MapTo<Notification, NotificationAppDto>());

        return Result<PagedList<NotificationAppDto>>.Success(
            new PagedList<NotificationAppDto>(notifications.Pagination, notificationDtos.ToList()));
    }
}
