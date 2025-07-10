using Domain.Models.Common;

namespace Domain.Models;

public class DifficultyProfileConfig : BaseEntity
{
    public int DifficultyProfileId { get; set; } // PK, FK
    public int DifficultyId { get; set; } // PK, FK
    public int DifficultyPercentage { get; set; }
    
    public Difficulty? Difficulty { get; set; }
    public DifficultyProfile? DifficultyProfile { get; set; }
}
