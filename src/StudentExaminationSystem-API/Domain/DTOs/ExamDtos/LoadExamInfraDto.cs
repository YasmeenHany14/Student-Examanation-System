  namespace Domain.DTOs.ExamDtos;

public class LoadExamInfraDto : BaseDto
{
    public int Id { get; set; }
    public int subjectId { get; set; }
    public DateTime examStartTime { get; set; }
    public DateTime examEndTime { get; set; }
    public IEnumerable<LoadExamQuestionInfraDto> questions { get; set; } = new List<LoadExamQuestionInfraDto>();
}
