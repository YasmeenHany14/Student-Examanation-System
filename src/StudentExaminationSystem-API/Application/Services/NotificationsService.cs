using Application.Common.Constants.Errors;
using Application.Common.Constants.Notifications;
using Application.Common.ErrorAndResults;
using Application.Contracts;
using Application.DTOs;
using Application.Mappers;
using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Shared.ResourceParameters;

namespace Application.Services;

public class NotificationsService(
    IUnitOfWork unitOfWork,
    INotificationsHub notificationsHub,
    IMapper mapper
    ): INotificationsService
{
    public async Task<Result<bool>> NotifyExamStartedAsync(int subjectId, string userId)
    {
        var subject = await unitOfWork.SubjectRepository.GetByIdAsync(subjectId);
        var student = await unitOfWork.StudentRepository.GetByIdAsync(userId);
        var message = string.Format(Notifications.ExamStarted, student?.Name, subject?.Name);
        
        var adminNotificationsResult = await GenerateAdminNotificationsAsync(message);
        if (!adminNotificationsResult.IsSuccess)
            return Result<bool>.Failure(adminNotificationsResult.Error);
        
        var notificationsDict = new Dictionary<string, NotificationAppDto>();
        foreach (var notification in adminNotificationsResult.Value)
            notificationsDict.Add(notification.UserId, mapper.Map<NotificationAppDto>(notification));
        
        await notificationsHub.SendAdminNotificationsAsync(notificationsDict);
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
        var adminNotificationsResult = await GenerateAdminNotificationsAsync(adminMsg);
        if (!adminNotificationsResult.IsSuccess)
            return Result<bool>.Failure(adminNotificationsResult.Error);
        
        var notificationsDict = new Dictionary<string, NotificationAppDto>();
        foreach (var notification in adminNotificationsResult.Value)
            notificationsDict.Add(notification.UserId, mapper.Map<NotificationAppDto>(notification));

        await notificationsHub.SendEvaluationCompletedAsync(student.UserId, mapper.Map<NotificationAppDto>(studentNotification));
        await notificationsHub.SendAdminNotificationsAsync(notificationsDict);
        
        return Result<bool>.Success(true);
    }
    
    public async Task<Result<bool>> MarkAllNotificationsAsReadAsync(string userId)
    {
        var notifications = await unitOfWork.NotificationsRepository.GetAllUnreadNotificationsAsync(userId);
        
        foreach (var n in notifications)
            n.IsRead = true;
        
        unitOfWork.NotificationsRepository.UpdateRangeAsync(notifications);
        
        var result = await unitOfWork.SaveChangesAsync();
        if (result <= 0)
            return Result<bool>.Failure(CommonErrors.InternalServerError());

        return Result<bool>.Success(true);
    }

    public async Task<Result<PagedList<NotificationAppDto>>> GetAllNotificationsAsync(
        BaseResourceParameters resourceParameters, string userId)
    {
        var notifications =
            await unitOfWork.NotificationsRepository.GetNotificationsByUserIdAsync(resourceParameters, userId);
        var notificationDtos = mapper.Map<IEnumerable<NotificationAppDto>>(notifications.Data);

        return Result<PagedList<NotificationAppDto>>.Success(
            new PagedList<NotificationAppDto>(notifications.Pagination, notificationDtos.ToList()));
    }

    private async Task<Result<IEnumerable<Notification>>> GenerateAdminNotificationsAsync(string message)
    {
        var adminIds = (await unitOfWork.UserExtensionsRepository.GetAdminUserIdsAsync()).ToList();
        var notifications = new List<Notification>();
        foreach(var adminId in adminIds)
            notifications.Add(new Notification(adminId, message));
        
        notifications = (await unitOfWork.NotificationsRepository.CreateRangeAsync(notifications)).ToList();
        var result = await unitOfWork.SaveChangesAsync();
        if (result <= 0)
            return Result<IEnumerable<Notification>>.Failure(CommonErrors.InternalServerError());
        return Result<IEnumerable<Notification>>.Success(notifications);
    }
}
