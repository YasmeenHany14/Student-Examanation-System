using Domain.Models.Common;

namespace Domain.Models;

public class DifficultyProfile : PrimaryKeyBaseEntity, ISoftDelete
{
    public string Name { get; set; } = string.Empty;
    
    public ICollection<DifficultyProfileConfig>? DifficultyProfileConfigs { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string DeletedBy { get; set; } = string.Empty;
}
