namespace Application.DTOs.AuthDtos;

public record LogoutRequest
{
    public string RefreshToken { get; init; }
}