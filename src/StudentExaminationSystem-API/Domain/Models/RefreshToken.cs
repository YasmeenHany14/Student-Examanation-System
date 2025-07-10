using Domain.Models.Common;

namespace Domain.Models;

public class RefreshToken : PrimaryKeyBaseEntity
{
    public string Token { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    public bool IsRevoked { get; set; }
    public bool IsUsed { get; set; }
    
     public virtual User? User { get; set; }
}
