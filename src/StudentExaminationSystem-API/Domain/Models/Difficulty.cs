using Domain.Models.Common;

namespace Domain.Models;

public class Difficulty : PrimaryKeyBaseEntity, ISoftDelete
{
    public int DifficultyId { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public ICollection<Question>? Questions { get; set; }
    public ICollection<DifficultyProfile>? DifficultyProfiles { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string DeletedBy { get; set; } = string.Empty;
}
