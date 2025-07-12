using System.Text.Json.Serialization;

namespace Application.DTOs.StudentDtos;

public class GetStudentAppDto
{
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public int? HiddenId { get; set; }
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int Age { get; set; }
    public DateTime JoinDate { get; set; }
}
