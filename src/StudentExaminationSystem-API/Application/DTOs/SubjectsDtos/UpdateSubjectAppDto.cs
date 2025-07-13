using Domain.DTOs;

namespace Application.DTOs.SubjectsDtos;

public class UpdateSubjectAppDto : BaseDto
{
    public string? Code { get; set; }
    public string? Name { get; set; }
}
