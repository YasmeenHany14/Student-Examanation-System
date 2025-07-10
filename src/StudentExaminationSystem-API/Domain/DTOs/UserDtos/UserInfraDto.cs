using Domain.Enums;

namespace Domain.DTOs.UserDtos;

public class UserInfraDto 
{
    public string Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly Birthdate { get; set; }
    public Gender Gender { get; set; }
    public string Email { get; set; } = string.Empty;
}
