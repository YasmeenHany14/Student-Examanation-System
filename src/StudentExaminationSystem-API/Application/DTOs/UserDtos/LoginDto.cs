namespace Application.DTOs.AuthDtos;

public record LoginDto
{
    public string Email { get; init; }
    public string Password { get; init; }
}
