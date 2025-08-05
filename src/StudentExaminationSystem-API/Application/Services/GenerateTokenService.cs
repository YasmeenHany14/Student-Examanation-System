using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Constants.Errors;
using Application.Common.ErrorAndResults;
using Application.Contracts;
using Domain.Models;
using Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

// Get claims from expired token (when access token is expired, but refresh token is still valid)
public class GenerateTokenService(
    IConfiguration configuration,
    IUserRepository userRepository)
    : IGenerateTokenService
{
    private async Task<List<Claim>> GetClaims(User user)
    {
        var roles = await userRepository.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }
    
    public async Task<Result<string>> GenerateAccessTokenAsync(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expires = configuration.GetValue<int>("Jwt:ExpireMinutes");
        var audience = configuration.GetValue<string>("Jwt:Audience");
        var issuer = configuration.GetValue<string>("Jwt:Issuer");
        var claims = await GetClaims(user);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expires),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        if (string.IsNullOrEmpty(token))
            return Result<string>.Failure(CommonErrors.CannotGenerateToken());
        return Result<string>.Success(token);
    }
    
    public Result<(string, DateTime)> GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var refreshToken = Convert.ToBase64String(randomNumber);
        var expires = DateTime.UtcNow.AddDays(configuration.GetValue<int>("Jwt:RefreshTokenExpireDays"));
        return Result<(string, DateTime)>.Success((refreshToken, expires));
    }
    
    public async Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidAudience = configuration["Jwt:Audience"],
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = configuration.GetValue<bool>("Jwt:ValidateIssuerSigningKey"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
            ValidateLifetime = false
        };

        var handler = new JsonWebTokenHandler();
    
        // Validate token and get result
        var result = await handler.ValidateTokenAsync(token, tokenValidationParameters);
        if (!result.IsValid)
            throw new SecurityTokenException("Invalid token");
        
        return new ClaimsPrincipal(result.ClaimsIdentity);
    }
}
