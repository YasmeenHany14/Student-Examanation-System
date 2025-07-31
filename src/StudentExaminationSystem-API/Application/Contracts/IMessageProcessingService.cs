using Application.DTOs;

namespace Application.Contracts;

public interface IMessageProcessingService
{
    Task ProcessExamEvaluationMessageAsync(ExamEvaluationDto data);
}
