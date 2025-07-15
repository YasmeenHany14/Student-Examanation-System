namespace Application.DTOs.SubjectExamConfigDtos;

public class GetSubjectExamConfigAppDto : AppBaseDto
{
    public int Id { get; set; }
    public int TotalQuestions { get; set; }
    public int DurationMinutes { get; set; }
    public int DifficultyProfileId { get; set; }
    public string DifficultyProfileSpecifications { get; set; } = string.Empty;
}
