using System.Security.Claims;
using Application.Common.Constants.Errors;
using Application.Common.ErrorAndResults;
using Application.Contracts;
using Application.DTOs.AuthDtos;
using Application.DTOs.UserDtos;
using Application.Mappers.UserMappers;
using Domain.Models;
using Domain.Repositories;

namespace Application.Services;

public class AuthService(
    IGenerateTokenService generateTokenService,
    IUnitOfWork unitOfWork) : IAuthService
{
    public async Task<Result<User>> RegisterAsync(CreateUserAppDto registerDto)
    {
        var user = registerDto.ToUser();
        var (id, identityResult) = await unitOfWork.UserRepository.CreateAsync(user, registerDto.Password);
        if (id == null || !identityResult.Succeeded)
            return Result<User>.Failure(CommonErrors.InternalServerError());

        return Result<User>.Success(user);
    }

    public async Task<Result<AuthTokensResponse>> LoginAsync(LoginDto userDto)
    {
        var user = await unitOfWork.UserRepository.FindByEmailAsync(userDto.Email);
        var isValid = user != null && await unitOfWork.UserRepository.CheckPasswordAsync(user, userDto.Password);
        if (!isValid)
            return Result<AuthTokensResponse>.Failure(CommonErrors.WrongCredentials());
        if (!user.IsActive)
            return Result<AuthTokensResponse>.Failure(AuthErrors.UserNotActive);
        
        var response = await GenerateTokenResponse(user);
        if (!response.IsSuccess)
            return Result<AuthTokensResponse>.Failure(response.Error);
        
        await unitOfWork.RefreshTokenRepository.AddAsync(new RefreshToken
        {
            Token = response.Value.RefreshToken,
            UserId = user.Id,
            ExpiryDate = response.Value.RefreshExpiresAt
        });
        
        var result = await unitOfWork.SaveChangesAsync();
        
        return result <= 0 ? Result<AuthTokensResponse>.Failure(CommonErrors.InternalServerError()) : response;
    }

    public async Task<Result<AuthTokensResponse>> RefreshTokenAsync(string accessToken, string refreshToken)
    {
        var principal = await generateTokenService.GetPrincipalFromExpiredToken(accessToken);
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            await unitOfWork.RefreshTokenRepository.RevokeToken(refreshToken);
            return Result<AuthTokensResponse>.Failure(CommonErrors.InvalidRefreshToken());
        }
        
        var user = await unitOfWork.UserRepository.FindByIdAsync(userId);
        if (user == null)
            return Result<AuthTokensResponse>.Failure(CommonErrors.InvalidRefreshToken());
            
        var token = await unitOfWork.RefreshTokenRepository.CheckTokenExistsByUserId(refreshToken, userId);
        if (token == null || token.ExpiryDate < DateTime.UtcNow || token.IsRevoked)
            return Result<AuthTokensResponse>.Failure(CommonErrors.InvalidRefreshToken());
        
        var response = await GenerateTokenResponse(user);
        if (!response.IsSuccess)
            return Result<AuthTokensResponse>.Failure(response.Error);
        
        unitOfWork.RefreshTokenRepository.ReplaceToken(token, response.Value.RefreshToken, response.Value.RefreshExpiresAt);
        await unitOfWork.SaveChangesAsync();

        return response;
    }

    private async Task<Result<AuthTokensResponse>> GenerateTokenResponse(User user)
    {
        var generatToken = await generateTokenService.GenerateAccessTokenAsync(user);
        var generateRefreshToken = generateTokenService.GenerateRefreshToken();

        if (!generatToken.IsSuccess)
            return Result<AuthTokensResponse>.Failure(generatToken.Error);
        
        return Result<AuthTokensResponse>.Success(new AuthTokensResponse(
            generatToken.Value,
            generateRefreshToken.Value.Item1,
            generateRefreshToken.Value.Item2));
    }

    public async Task<Result> LogoutAsync(string refreshToken)
    {
        var tokenExists = await unitOfWork.RefreshTokenRepository.CheckTokenExists(refreshToken);
        if (!tokenExists)
            return Result.Failure(CommonErrors.InvalidRefreshToken());
        
        await unitOfWork.RefreshTokenRepository.RevokeToken(refreshToken);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
    
    public async Task<Result>AddToRoleAsync(string role, User? sentUser, string? email)
    {
        var user = sentUser;
        if (email != null) // handle use case where this is method is called from assignRole endpoint
            user = await unitOfWork.UserRepository.FindByEmailAsync(email);
        
        if (user == null)
            return Result.Failure(CommonErrors.NotFound());
        
        var roles = await unitOfWork.UserRepository.GetRolesAsync(user);
        foreach (var r in roles)
        {
            await unitOfWork.UserRepository.RemoveFromRoleAsync(user, r); // TODO: Think about allowing multiple roles
        }
        
        var result = await unitOfWork.UserRepository.AddToRoleAsync(user, role);
        
        return result.Succeeded? Result.Success() : Result.Failure(CommonErrors.InternalServerError());
    }
}
