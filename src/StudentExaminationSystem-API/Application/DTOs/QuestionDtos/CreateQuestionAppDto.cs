using System.Text.Json.Serialization;
using Application.DTOs.QuestionChoiceDtos;

namespace Application.DTOs.QuestionDtos;

public class CreateQuestionAppDto : AppBaseDto
{
    public int SubjectId { get; set; }
    public string Content { get; set; } = string.Empty;
    public int DifficultyId { get; set; }
    public ICollection<CreateQuestionChoiceAppDto> QuestionChoices { get; set; } = new List<CreateQuestionChoiceAppDto>();
}
