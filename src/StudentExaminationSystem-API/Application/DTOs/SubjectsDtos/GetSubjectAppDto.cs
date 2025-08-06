using System.Text.Json.Serialization;

namespace Application.DTOs.SubjectsDtos;

public class GetSubjectAppDto : AppBaseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Code { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? HasConfiguration { get; set; }
}
