namespace Application.DTOs.ExamDtos;

public class GetExamHistoryAppDto : AppBaseDto
{
    public int Id { get; set; }
    public int SubjectId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public DateTime ExamDate { get; set; }
    public int FinalScore { get; set; }
    public bool Passed { get; set; }
    public int ExamStatus { get; set; }
    // public IEnumerable<GetQuestionHistoryAppDto> Questions { get; set; }
}
