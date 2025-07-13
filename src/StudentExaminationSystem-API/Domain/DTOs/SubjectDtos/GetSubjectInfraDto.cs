namespace Domain.DTOs.SubjectDtos;

public class GetSubjectInfraDto : BaseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
}
