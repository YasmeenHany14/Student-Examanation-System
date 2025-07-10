namespace Application.DTOs.AuthDtos;

public class AuthTokensResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshExpiresAt { get; set; }
    
    public AuthTokensResponse(string accessToken, string refreshToken, DateTime refreshExpiresAt)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        RefreshExpiresAt = refreshExpiresAt;
    }
}
