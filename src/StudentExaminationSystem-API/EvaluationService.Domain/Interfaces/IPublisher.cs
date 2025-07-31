namespace EvaluationService.Domain.Interfaces;

public interface IPublisher
{
    Task PublishEvaluationAsync(int examId, int totalScore);
}
