using Domain.Enums;

namespace Application.DTOs.UserDtos;

public class CreateUserServiceDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public DateOnly Birthdate { get; set; }
    public Gender Gender { get; set; }
}
