using Domain.Models.Common;

namespace Domain.Models;

public class GeneratedExam : PrimaryKeyBaseEntity
{
    public int StudentId { get; set; } // FK
    public int SubjectId { get; set; } // FK
    public DateTime SubmittedAt { get; set; }
    public bool IsCompleted { get; set; }
    public int FinalScore { get; set; }
    
    public  Student? Student { get; set; }
    public  Subject? Subject { get; set; }
}
