using System.Text.Json.Serialization;

namespace Domain.DTOs;

public class GetDifficultyProfileInfraDto : BaseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? EasyQuestionsPercent { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? MediumQuestionsPercent { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? HardQuestionsPercent { get; set; }
}
