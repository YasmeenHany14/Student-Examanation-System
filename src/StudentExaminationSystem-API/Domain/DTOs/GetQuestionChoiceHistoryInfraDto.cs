namespace Domain.DTOs;

public class GetQuestionChoiceHistoryInfraDto : BaseDto
{
    public string Content { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public bool IsSelected { get; set; }
}
