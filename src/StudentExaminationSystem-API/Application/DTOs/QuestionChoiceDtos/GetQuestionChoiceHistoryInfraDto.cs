using Domain.DTOs;

namespace Application.DTOs.QuestionChoiceDtos;

public class GetQuestionChoiceHistoryAppDto : AppBaseDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public bool IsSelected { get; set; }
}
