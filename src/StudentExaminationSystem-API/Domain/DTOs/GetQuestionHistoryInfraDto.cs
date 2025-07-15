namespace Domain.DTOs;

public class GetQuestionHistoryInfraDto : BaseDto
{
    public int QuestionId { get; set; }
    public string Question { get; set; } = string.Empty;
    public IEnumerable<GetQuestionChoiceHistoryInfraDto> Choices { get; set; } = new List<GetQuestionChoiceHistoryInfraDto>();
}
