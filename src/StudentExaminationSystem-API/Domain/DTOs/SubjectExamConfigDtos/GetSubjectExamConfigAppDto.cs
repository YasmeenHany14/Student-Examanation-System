namespace Domain.DTOs.SubjectExamConfigDtos;

public class GetSubjectExamConfigInfraDto : BaseDto
{
    public int Id { get; set; }
    public int TotalQuestions { get; set; }
    public int DurationMinutes { get; set; }
    public int DifficultyProfileId { get; set; }
    public string DifficultyProfileSpecifications { get; set; } = string.Empty;
}
