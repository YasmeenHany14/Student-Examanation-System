namespace Domain.DTOs;

public class GetQuestionChoiceInfraDto : BaseDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}
