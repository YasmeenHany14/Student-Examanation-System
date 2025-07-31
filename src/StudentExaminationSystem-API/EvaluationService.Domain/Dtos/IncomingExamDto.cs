namespace EvaluationService.Domain.Dtos;

public class IncomingExamDto
{
    public int ExamId { get; set; }
    public IEnumerable<EvaluateQuestionDto> EvaluateQuestions { get; set; }
    public IEnumerable<CorrectAnswersInfraDto> CorrectAnswers { get; set; }
}
