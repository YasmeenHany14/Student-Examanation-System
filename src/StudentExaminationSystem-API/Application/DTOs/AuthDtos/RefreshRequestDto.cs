namespace Application.DTOs.AuthDtos;

public record RefreshRequestDto
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}
