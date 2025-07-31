namespace Domain.DTOs.ExamDtos;

public class CorrectAnswersInfraDto : BaseDto
{
    public int QuestionId { get; set; }
    public int CorrectAnswerId { get; set; }
}
