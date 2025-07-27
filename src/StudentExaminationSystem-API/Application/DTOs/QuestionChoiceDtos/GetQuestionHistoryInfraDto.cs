using Domain.DTOs;
namespace Application.DTOs.QuestionChoiceDtos;

public class GetQuestionHistoryAppDto : AppBaseDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public IEnumerable<GetQuestionChoiceHistoryAppDto> Choices { get; set; } = new List<GetQuestionChoiceHistoryAppDto>();
}
