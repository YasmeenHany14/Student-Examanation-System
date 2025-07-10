using Domain.Models.Common;

namespace Domain.Models;

public class Employee : PrimaryKeyBaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string EmployeeId { get; set; } = string.Empty;
    public DateOnly HireDate { get; set; }
    
    public User? User { get; set; }
}
