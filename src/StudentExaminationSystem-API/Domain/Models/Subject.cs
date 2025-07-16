using Domain.Models.Common;

namespace Domain.Models;

public class Subject : PrimaryKeyBaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    
    public ICollection<Question>? Questions { get; set; } 
    public ICollection<GeneratedExam>? GeneratedExams { get; set; }
    public SubjectExamConfig? SubjectExamConfig { get; set; }
    public ICollection<StudentSubject>? StudentSubjects { get; set; }
}

