using Domain.Enums;
using Domain.Models.Common;

namespace Domain.Models;

public class GeneratedExam : PrimaryKeyBaseEntity, IAuditDateOnly
{
    public int StudentId { get; set; } // FK
    public int SubjectId { get; set; } // FK
    public DateTime? SubmittedAt { get; set; }
    public ExamStatus ExamStatus { get; set; } = ExamStatus.Running;
    public int ExamTotalScore { get; set; }
    public int StudentScore { get; set; }
    
    public  Student? Student { get; set; }
    public  Subject? Subject { get; set; }
    public ICollection<AnswerHistory>? QuestionHistory { get; set; } = new List<AnswerHistory>();
}
