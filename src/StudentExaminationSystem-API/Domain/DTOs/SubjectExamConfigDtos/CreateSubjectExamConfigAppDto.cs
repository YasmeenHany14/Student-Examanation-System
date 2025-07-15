namespace Domain.DTOs.SubjectExamConfigDtos;

public class CreateSubjectExamConfigInfraDto : BaseDto 
{
    public int SubjectId { get; set; }
    public int TotalQuestions { get; set; }
    public int DurationMinutes { get; set; }
    public int DifficultyProfileId { get; set; }
}
