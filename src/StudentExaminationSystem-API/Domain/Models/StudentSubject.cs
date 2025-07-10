using Domain.Models.Common;

namespace Domain.Models;

public class StudentSubject : BaseEntity
{
    public int StudentId { get; set; } // PK, FK
    public int SubjectId { get; set; } // PK, FK
    
    public Student? Student { get; set; }
    public Subject? Subject { get; set; }
}
