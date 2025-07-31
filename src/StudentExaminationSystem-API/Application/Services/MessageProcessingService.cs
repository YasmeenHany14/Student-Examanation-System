using Application.Contracts;
using Application.DTOs;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class MessageProcessingService(
    IExamService examService,
    INotificationsService notificationsService,
    ILogger<MessageProcessingService> logger
    ) : IMessageProcessingService
{
    public async Task ProcessExamEvaluationMessageAsync(ExamEvaluationDto data)
    {
        var result = await examService.SaveExamEvaluationAsync(data);
        if (!result.IsSuccess)
        {
            logger.LogError("Failed to process exam evaluation message: {Error}", result.Error);
            return;
        }
        
        var (subjectId, studentId) = result.Value;
        await notificationsService.NotifyExamEvaluatedAsync(subjectId, studentId, data.TotalScore);
    }
}
