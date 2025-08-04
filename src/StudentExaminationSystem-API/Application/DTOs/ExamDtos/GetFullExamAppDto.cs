using Application.DTOs.QuestionChoiceDtos;

namespace Application.DTOs.ExamDtos;

public class GetFullExamAppDto : AppBaseDto
{
    public int FinalScore { get; set; }
    public bool Passed { get; set; }
    public IEnumerable<GetQuestionHistoryAppDto> Questions { get; set; }
}
