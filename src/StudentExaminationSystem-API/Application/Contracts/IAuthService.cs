using Application.Common.ErrorAndResults;
using Application.DTOs.AuthDtos;
using Application.DTOs.UserDtos;
using Domain.Models;

namespace Application.Contracts;

public interface IAuthService
{
    Task<Result<User>> RegisterAsync(CreateUserAppDto registerDto);
    Task<Result<AuthTokensResponse>> LoginAsync(LoginDto userDto);
    Task<Result<AuthTokensResponse>> RefreshTokenAsync(string accessToken, string refreshToken);
    public Task<Result> LogoutAsync(string refreshToken);
    Task<Result> AddToRoleAsync(string role, User? user, string? email);
}
