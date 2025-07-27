namespace Application.DTOs.QuestionChoiceDtos;

public class LoadExamQuestionAppDto : AppBaseDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int QuestionOrder { get; set; }
    public IEnumerable<LoadExamChoiceAppDto> Choices { get; set; } = new List<LoadExamChoiceAppDto>();
}
