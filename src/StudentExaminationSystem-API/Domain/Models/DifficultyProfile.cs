using Domain.Models.Common;

namespace Domain.Models;

public class DifficultyProfile : PrimaryKeyBaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int EasyPercentage { get; set; } = 0;
    public int MediumPercentage { get; set; } = 0;
    public int HardPercentage { get; set; } = 0;
}
