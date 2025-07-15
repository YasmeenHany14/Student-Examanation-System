namespace Application.DTOs.QuestionChoiceDtos;

public class LoadExamQuestionAppDto : AppBaseDto
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public int QuestionOrder { get; set; }
    public IEnumerable<LoadExamChoiceAppDto> Choices { get; set; } = new List<LoadExamChoiceAppDto>();
}
