using Domain.Enums;

namespace Domain.DTOs.ExamDtos;

public class LoadExamQuestionInfraDto : BaseDto
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public Difficulty? Difficulty { get; set; }
    public IEnumerable<LoadExamChoiceInfraDto> Choices { get; set; } = new List<LoadExamChoiceInfraDto>();
}
