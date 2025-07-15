namespace Domain.DTOs;

public class GetFullExamInfraDto : BaseDto
{
    public string userId { get; set; }
    public int FinalScore { get; set; }
    public IEnumerable<GetQuestionHistoryInfraDto> QuestionHistory { get; set; }
}
