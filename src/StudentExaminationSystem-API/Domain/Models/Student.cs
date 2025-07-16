using Domain.Models.Common;

namespace Domain.Models;

public class Student : PrimaryKeyBaseEntity
{
    public string UserId { get; set; } //FK
    public string StudentId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    
    public User? User { get; set; }
    public ICollection<GeneratedExam>? GeneratedExams { get; set; }
    public ICollection<StudentSubject>? StudentSubjects { get; set; }
}
