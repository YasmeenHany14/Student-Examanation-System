using System.Text.Json.Serialization;
using Application.DTOs.QuestionChoiceDtos;

namespace Application.DTOs.QuestionDtos;

public class CreateQuestionAppDto : AppBaseDto
{
    public int CourseId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public int QuestionTypeId { get; set; }
    public ICollection<CreateQuestionChoiceAppDto> QuestionChoices { get; set; } = new List<CreateQuestionChoiceAppDto>();
}
