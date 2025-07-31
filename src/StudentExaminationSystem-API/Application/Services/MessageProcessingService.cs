using Application.Contracts;
using Application.DTOs;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class MessageProcessingService(
    IExamService examService,
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
        // here comes signlrlR logic to notify the user about the evaluation result
        
    }
}
