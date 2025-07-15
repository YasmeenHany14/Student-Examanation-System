namespace Application.DTOs.QuestionChoiceDtos;

public class LoadExamChoiceAppDto : AppBaseDto
{
    public int ChoiceId { get; set; }
    public string ChoiceText { get; set; } = string.Empty;
    public bool IsSelected { get; set; }
}
