namespace Domain.DTOs.QuestionDtos;

public class GetQuestionChoiceInfraDto : BaseDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}
