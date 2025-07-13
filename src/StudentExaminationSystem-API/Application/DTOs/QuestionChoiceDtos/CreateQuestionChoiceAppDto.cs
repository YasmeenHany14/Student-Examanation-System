using System.Text.Json.Serialization;

namespace Application.DTOs.QuestionChoiceDtos;

public class CreateQuestionChoiceAppDto : AppBaseDto
{
    [JsonPropertyName("ChoiceText")]
    public string Content { get; set; }
    [JsonPropertyName("IsAnswer")]
    public bool IsCorrect { get; set; }
}
