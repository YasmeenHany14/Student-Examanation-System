using Domain.Models.Common;

namespace Domain.Models;

public class AnswerHistory : BaseEntity
{
    public int GeneratedExamId { get; set; } // PK, FK
    public int QuestionId { get; set; } // PK, FK
    public int QuestionChoiceId { get; set; } // FK
    public bool IsCorrect { get; set; }
    public int DisplayOrder { get; set; }
    
    public Question? Question { get; set; }
    public QuestionChoice? QuestionChoice { get; set; }
}
