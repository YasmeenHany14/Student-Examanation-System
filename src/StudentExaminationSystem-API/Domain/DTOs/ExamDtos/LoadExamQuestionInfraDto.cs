namespace Domain.DTOs.ExamDtos;

public class LoadExamQuestionInfraDto : BaseDto
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public IEnumerable<LoadExamChoiceInfraDto> Choices { get; set; } = new List<LoadExamChoiceInfraDto>();
}
