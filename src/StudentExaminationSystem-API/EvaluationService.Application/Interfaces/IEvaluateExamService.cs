using EvaluationService.Domain.Dtos;

namespace EvaluationService.Application.Interfaces;

public interface IEvaluateExamService
{
    Task  EvaluateExamAsync(IncomingExamDto examDto);
}
