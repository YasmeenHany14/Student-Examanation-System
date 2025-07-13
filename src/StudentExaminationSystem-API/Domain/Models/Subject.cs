using Domain.Models.Common;

namespace Domain.Models;

public class Subject : PrimaryKeyBaseEntity, ISoftDelete
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    
    public ICollection<Question>? Questions { get; set; } 
    public ICollection<GeneratedExam>? GeneratedExams { get; set; }
    public SubjectExamConfig? SubjectExamConfig { get; set; }

    public bool IsDeleted { get; set; } = false;
    public string DeletedBy { get; set; } = string.Empty;
}

