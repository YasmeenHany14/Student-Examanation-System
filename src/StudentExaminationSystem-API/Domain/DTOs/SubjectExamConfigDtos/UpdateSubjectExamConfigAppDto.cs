namespace Domain.DTOs.SubjectExamConfigDtos;

public class UpdateSubjectExamConfigInfraDto : BaseDto
{
    public int Id { get; set; }
    public int SubjectId { get; set; }
    public int TotalQuestions { get; set; }
    public int DurationMinutes { get; set; }
    public int DifficultyProfileId { get; set; }
}
