using System.Text.Json.Serialization;

namespace Application.DTOs.StudentDtos;

public class GetStudentByIdAppDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateOnly? Birthdate { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? JoinDate { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ICollection<CommonDtos.DropdownAppDto>? Courses { get; set; } = new List<CommonDtos.DropdownAppDto>();
}
