using Domain.Enums;
using Domain.Models.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly Birthdate { get; set; }
    public bool IsActive { get; set; } = true;
    
    public Gender Gender { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
