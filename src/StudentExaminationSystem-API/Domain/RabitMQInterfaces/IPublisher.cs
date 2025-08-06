using Domain.DTOs.ExamDtos;

namespace Domain.RabitMQInterfaces;

public interface IPublisher
{
    Task PublishExamAsync(
        IEnumerable<CorrectAnswersInfraDto> answersForExam,
        int examId,
        IEnumerable<EvaluateQuestionDto> evaluationQuestions);
}
