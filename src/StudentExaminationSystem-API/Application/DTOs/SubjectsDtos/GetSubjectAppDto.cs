namespace Application.DTOs.SubjectsDtos;

public class GetSubjectAppDto : AppBaseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Code { get; set; }
}

