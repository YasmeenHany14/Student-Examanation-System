namespace Domain.DTOs;

public class GetQuestionHistoryInfraDto : BaseDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public IEnumerable<GetQuestionChoiceHistoryInfraDto> Choices { get; set; } = new List<GetQuestionChoiceHistoryInfraDto>();
}
