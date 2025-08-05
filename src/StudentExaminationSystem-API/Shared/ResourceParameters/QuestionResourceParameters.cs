namespace Shared.ResourceParameters;

public class QuestionResourceParameters : BaseResourceParameters
{
    public int SubjectId { get; set; }
    public int? DifficultyId { get; set; }
    public string? SearchQuery { get; set; }
    
    // try to filter on it later
    // public bool? IsActive { get; set; }
}
