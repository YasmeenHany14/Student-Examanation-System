namespace Application.DTOs.StudentDtos;

public class GetStudentByIdAppDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateOnly Birthdate { get; set; }
    public DateTime JoinDate { get; set; }
    public ICollection<CommonDtos.DropdownAppDto> Courses { get; set; } = new List<CommonDtos.DropdownAppDto>();
}
