using EvaluationService.Application.Interfaces;
using EvaluationService.Domain.Dtos;
using EvaluationService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvaluationService.Application.Services;

public class MessageProcessingService : IMessageProcessingService
{
    private readonly IEvaluateExamService _evaluateExamService;
    private readonly ILogger<MessageProcessingService> _logger;

    public MessageProcessingService(IEvaluateExamService evaluateExamService, ILogger<MessageProcessingService> logger)
    {
        _evaluateExamService = evaluateExamService;
        _logger = logger;
    }

    public async Task ProcessExamMessageAsync(IncomingExamDto data)
    {
        try
        {
            _logger.LogInformation("Processing exam message for exam ID: {ExamId}", data.ExamId);
            await _evaluateExamService.EvaluateExamAsync(data);
            _logger.LogInformation("Successfully processed exam message for exam ID: {ExamId}", data.ExamId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing exam message for exam ID: {ExamId}", data.ExamId);
            throw;
        }
    }
}
