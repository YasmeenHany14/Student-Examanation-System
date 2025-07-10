using Domain.Models.Common;

namespace Domain.Models;

public class Student : PrimaryKeyBaseEntity, ISoftDelete
{
    public string UserId { get; set; } //FK
    public string StudentId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    
    public User? User { get; set; }
    public ICollection<GeneratedExam>? GeneratedExams { get; set; }
    
    // TODO: many to many nav??
    public ICollection<StudentSubject>? StudentSubjects { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string DeletedBy { get; set; } = string.Empty;
}
