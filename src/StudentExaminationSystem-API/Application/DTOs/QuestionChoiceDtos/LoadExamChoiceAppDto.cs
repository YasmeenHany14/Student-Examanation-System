namespace Application.DTOs.QuestionChoiceDtos;

public class LoadExamChoiceAppDto : AppBaseDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsSelected { get; set; }
}
