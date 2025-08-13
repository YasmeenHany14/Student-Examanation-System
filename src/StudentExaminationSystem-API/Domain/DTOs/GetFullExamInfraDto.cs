using Domain.Enums;

namespace Domain.DTOs;

public class GetFullExamInfraDto : BaseDto
{
    public string userId { get; set; }
    public string SubjectName { get; set; }
    public int FinalScore { get; set; }
    public ExamStatus ExamStatus { get; set; }
    public IEnumerable<GetQuestionHistoryInfraDto> Questions { get; set; }
}
