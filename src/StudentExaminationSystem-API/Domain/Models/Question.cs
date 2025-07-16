using Domain.Enums;
using Domain.Models.Common;

namespace Domain.Models;

public class Question : PrimaryKeyBaseEntity
{
    public int SubjectId { get; set; } // FK
    public string Content { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    
    public Subject? Subject { get; set; }
    public Difficulty? Difficulty { get; set; }
    public ICollection<QuestionChoice>? Choices { get; set; }
    public ICollection<AnswerHistory>? AnswerHistories { get; set; }
}
