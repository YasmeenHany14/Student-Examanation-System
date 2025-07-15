namespace Application.DTOs.SubjectExamConfigDtos;

public class CreateSubjectExamConfig : AppBaseDto
{
    public int? TotalQuestions { get; set; }
    public int? DurationMinutes { get; set; }
    public int? DifficultyProfileId { get; set; }
}
