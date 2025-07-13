using Domain.Enums;
using Domain.Models.Common;

namespace Domain.Models;

public class Question : PrimaryKeyBaseEntity, ISoftDelete
{
    public int SubjectId { get; set; } // FK
    public string Content { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    
    public Subject? Subject { get; set; }
    public Difficulty? Difficulty { get; set; }
    public ICollection<QuestionChoice>? Choices { get; set; }
    public ICollection<AnswerHistory>? AnswerHistories { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string DeletedBy { get; set; } = string.Empty;
}
