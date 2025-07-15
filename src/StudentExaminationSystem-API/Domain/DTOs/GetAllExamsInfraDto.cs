namespace Domain.DTOs;

public class GetAllExamsInfraDto : BaseDto
{
    public int Id { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public DateTime ExamDate { get; set; }
    public int FinalScore { get; set; }
    public bool Passed { get; set; }
}
