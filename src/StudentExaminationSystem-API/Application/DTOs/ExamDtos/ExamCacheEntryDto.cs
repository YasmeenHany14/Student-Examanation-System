namespace Application.DTOs.ExamDtos;

public class ExamCacheEntryDto
{
    public int ExamId { get; set; }
    public int SubjectId { get; set; }
    public DateTime ExamEndTime { get; set; }

    public ExamCacheEntryDto(int examId, int subjectId, int durationMinutes)
    {
        ExamId = examId;
        SubjectId = subjectId;
        ExamEndTime = DateTime.UtcNow.AddMinutes(durationMinutes);
    }
}
