namespace Application.DTOs.UserDtos;

public record LoginDto
{
    public string Email { get; init; }
    public string Password { get; init; }
}
