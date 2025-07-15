namespace Application.DTOs.DifficultyProfileDtos;

public class CreateUpdateDifficultyProfileAppDto : AppBaseDto
{
    public string? Name { get; set; }
    public int? EasyQuestionsPercent { get; set; }
    public int? MediumQuestionsPercent { get; set; }
    public int? HardQuestionsPercent { get; set; }
}
