using System.Security.Claims;
using Application.Common.ErrorAndResults;
using Application.DTOs.UserDtos;
using Domain.Models;

namespace Application.Contracts;

public interface IGenerateTokenService
{
    Task<Result<string>> GenerateAccessTokenAsync(User user);
    Result<(string, DateTime)> GenerateRefreshToken();
    Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
}
