using System.Text.Json.Serialization;

namespace Application.DTOs.QuestionDtos;

public class SubmitExamAnswerAppDto
{
    public int QuestionId { get; set; }
    public int? ChoiceId { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public bool IsCorrect { get; set; } = false;
}
