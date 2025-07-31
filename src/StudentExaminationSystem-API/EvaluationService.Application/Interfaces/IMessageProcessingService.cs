using EvaluationService.Domain.Dtos;

namespace EvaluationService.Application.Interfaces;

public interface IMessageProcessingService
{
    Task ProcessExamMessageAsync(IncomingExamDto data);
}
