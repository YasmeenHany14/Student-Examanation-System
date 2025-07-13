namespace Domain.DTOs.QuestionDtos;

public class GetQuestionInfraDto : BaseDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    
}
