namespace Domain.DTOs.StudentDtos;

public class GetStudentByIdInfraDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateOnly Birthdate { get; set; }
    public DateTime JoinDate { get; set; }
    public ICollection<CommonDtos.DropdownInfraDto> Courses { get; set; } = new List<CommonDtos.DropdownInfraDto>();
}
