using Domain.DTOs;

namespace Application.DTOs.QuestionChoiceDtos;

public class GetQuestionChoiceHistoryAppDto : AppBaseDto
{
    public string ChoiceText { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public bool IsSelected { get; set; }
}
