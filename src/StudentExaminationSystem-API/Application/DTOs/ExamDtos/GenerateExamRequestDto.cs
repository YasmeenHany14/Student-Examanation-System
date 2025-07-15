namespace Application.DTOs.ExamDtos;

public class GenerateExamRequestDto
{
    public int SubjectId { get; set; }
    public int UserId { get; set; }

    public GenerateExamRequestDto(int subjectId, int userId)
    {
        SubjectId = subjectId;
        UserId = userId;
    }
}
