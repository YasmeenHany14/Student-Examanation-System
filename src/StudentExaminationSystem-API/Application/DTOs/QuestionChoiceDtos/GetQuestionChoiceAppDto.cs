namespace Application.DTOs.QuestionChoiceDtos;

public class GetQuestionChoiceAppDto : AppBaseDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}
