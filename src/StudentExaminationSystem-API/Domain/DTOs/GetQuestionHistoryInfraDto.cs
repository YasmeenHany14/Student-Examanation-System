namespace Domain.DTOs;

public class GetQuestionHistoryInfraDto : BaseDto
{
    public string Question { get; set; } = string.Empty;
    public IEnumerable<GetQuestionChoiceInfraDto> Choices { get; set; } = new List<GetQuestionChoiceInfraDto>();
}