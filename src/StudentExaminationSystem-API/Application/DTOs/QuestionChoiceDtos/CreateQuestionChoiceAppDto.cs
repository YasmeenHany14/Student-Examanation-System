using System.Text.Json.Serialization;

namespace Application.DTOs.QuestionChoiceDtos;

public class CreateQuestionChoiceAppDto : AppBaseDto
{
    public string Content { get; set; }
    public bool IsCorrect { get; set; }
}
