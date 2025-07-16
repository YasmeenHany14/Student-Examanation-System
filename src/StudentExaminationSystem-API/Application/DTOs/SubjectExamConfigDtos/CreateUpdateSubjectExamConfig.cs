namespace Application.DTOs.SubjectExamConfigDtos;

public class CreateUpdateSubjectExamConfig : AppBaseDto
{
    public int? TotalQuestions { get; set; }
    public int? DurationMinutes { get; set; }
    public int? DifficultyProfileId { get; set; }
}
