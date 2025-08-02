using EvaluationService.Application.Interfaces;
using EvaluationService.Domain.Dtos;
using EvaluationService.Domain.Interfaces;

namespace EvaluationService.Application.Services;

public class EvaluateExamService(IPublisher publisher) : IEvaluateExamService
{
    public async Task EvaluateExamAsync(IncomingExamDto examDto)
    {
        int totalScore = 0;
        foreach(var question in examDto.EvaluateQuestions)
        {
            var correctAnswer = examDto.CorrectAnswers
                .FirstOrDefault(c => c.QuestionId == question.QuestionId)?.CorrectAnswerId;
        
            if (correctAnswer.HasValue && question.AnswerId == correctAnswer.Value)
            {
                totalScore += 1;
            }
        }

        await publisher.PublishEvaluationAsync(examDto.ExamId, totalScore);
    }
}
