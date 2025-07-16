using Domain.Models.Common;

namespace Domain.Models;

public class QuestionChoice : PrimaryKeyBaseEntity
{
    public int QuestionId { get; set; } // FK
    public string Content { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    
    public Question? Question { get; set; }
    public ICollection<AnswerHistory>? AnswerHistories { get; set; }
}
