namespace Domain.DTOs;

public class GetQuestionInfraDto : BaseDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int? SubjectId { get; set; }
    public int? DifficultyId { get; set; }
    public bool? IsActive { get; set; }
    public IEnumerable<GetQuestionChoiceInfraDto> Choices { get; set; } = new List<GetQuestionChoiceInfraDto>();
}
