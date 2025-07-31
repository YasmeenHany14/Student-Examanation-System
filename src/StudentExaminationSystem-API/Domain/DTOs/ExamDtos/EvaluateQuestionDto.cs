namespace Domain.DTOs.ExamDtos;

public class EvaluateQuestionDto : BaseDto
{
    public int QuestionId { get; set; }
    public int AnswerId { get; set; }
}
