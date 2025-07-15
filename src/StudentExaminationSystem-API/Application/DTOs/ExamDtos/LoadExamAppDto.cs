 using Application.DTOs.QuestionChoiceDtos;

 namespace Application.DTOs.ExamDtos;

public class LoadExamAppDto : AppBaseDto
{
    public int ExamId { get; set; }
    public int SubjectId { get; set; }
    public DateTime ExamEndTime { get; set; }
    public IEnumerable<LoadExamQuestionAppDto> Questions { get; set; } = new List<LoadExamQuestionAppDto>();
}
