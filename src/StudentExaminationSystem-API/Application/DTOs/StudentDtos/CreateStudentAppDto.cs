using Application.DTOs.UserDtos;
using Domain.Enums;

namespace Application.DTOs.StudentDtos;

public class CreateStudentAppDto : CreateUserAppDto
{
    public DateTime JoinDate { get; set; }
    public IEnumerable<int>? CourseIds { get; set; }
}
