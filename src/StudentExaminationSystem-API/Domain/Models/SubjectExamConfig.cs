using Domain.Models.Common;

namespace Domain.Models;

public class SubjectExamConfig : BaseEntity
{
    public int SubjectId { get; set; } // PK, FK
    public int TotalQuestions { get; set; }
    public int DurationMinutes { get; set; }
    public int DifficultyProfileId { get; set; } // FK
    
    public DifficultyProfile? DifficultyProfile { get; set; }
    public Subject? Subject { get; set; }
}
