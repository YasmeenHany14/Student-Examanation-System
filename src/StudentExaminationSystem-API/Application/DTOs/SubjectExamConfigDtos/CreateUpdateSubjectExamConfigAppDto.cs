namespace Application.DTOs.SubjectExamConfigDtos;

public class CreateUpdateSubjectExamConfigAppDto : AppBaseDto 
{
    public int? TotalQuestions { get; set; }
    public int? DurationMinutes { get; set; }
    public int? DifficultyProfileId { get; set; }
}
