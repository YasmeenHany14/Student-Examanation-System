using System.Text.Json.Serialization;

namespace Application.DTOs.DifficultyProfileDtos;

public class GetDifficultyProfileAppDto : AppBaseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? EasyQuestionsPercent { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? MediumQuestionsPercent { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? HardQuestionsPercent { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? DifficultySpecification { get; set; }
}
