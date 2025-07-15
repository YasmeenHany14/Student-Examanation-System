using Application.DTOs.QuestionDtos;

namespace Application.DTOs.ExamDtos;

public class SubmitExamAppDto : AppBaseDto
{
    public int ExamId { get; set; }
    public int SubjectId { get; set; }
    public IEnumerable<SubmitExamAnswerAppDto> Answers { get; set; } = new List<SubmitExamAnswerAppDto>();
}
