using System.Text.Json.Serialization;
using Application.DTOs.QuestionChoiceDtos;

namespace Application.DTOs.QuestionDtos;

public class GetQuestionAppDto : AppBaseDto
{
    // used for get all questions
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? SubjectId { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? DifficultyId { get; set; }
    public bool? IsActive { get; set; }
    public IEnumerable<GetQuestionChoiceAppDto> Choices { get; set; }
}
