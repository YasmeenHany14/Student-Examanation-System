namespace Domain.DTOs;

public class GetStudentByIdInfraDto
{
    public string Id { get; set; } = string.Empty;
    public int? HiddenId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool? IsActive { get; set; }
    public DateOnly Birthdate { get; set; }
    public DateTime JoinDate { get; set; }
    public ICollection<DropdownInfraDto> Courses { get; set; } = new List<DropdownInfraDto>();
}
