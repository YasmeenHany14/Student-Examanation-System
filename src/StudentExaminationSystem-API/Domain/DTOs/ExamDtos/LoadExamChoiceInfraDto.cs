namespace Domain.DTOs.ExamDtos;

public class LoadExamChoiceInfraDto : BaseDto
{
    public int ChoiceId { get; set; }
    public string ChoiceText { get; set; } = string.Empty;
    public bool IsSelected { get; set; }
}
