using EvaluationService.Application.Interfaces;
using EvaluationService.Domain.Dtos;
using EvaluationService.Domain.Interfaces;

namespace EvaluationService.Application.Services;

public class EvaluateExamService(IPublisher publisher) : IEvaluateExamService
{
    public async Task EvaluateExamAsync(IncomingExamDto examDto)
    {
        int totalScore = 0;
        examDto.EvaluateQuestions.Select(q =>
        {
            var correctAnswer = examDto.CorrectAnswers
                .FirstOrDefault(c => c.QuestionId == q.QuestionId)?.CorrectAnswerId;

            if (correctAnswer.HasValue && q.AnswerId == correctAnswer.Value)
            {
                totalScore += 1;
            }

            return q;
        });

        await publisher.PublishEvaluationAsync(examDto.ExamId, totalScore);
    }
}
