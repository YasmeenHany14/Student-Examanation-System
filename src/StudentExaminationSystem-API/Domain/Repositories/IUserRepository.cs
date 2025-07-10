using Domain.DTOs.UserDtos;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task<(string?, IdentityResult)> CreateAsync(CreateUserInfraDto infraDto);
    Task<bool> CheckPasswordAsync(UserInfraDto userInfraDto, string password);
    Task<bool> CheckPasswordAsync(string email, string password);

    Task<UserInfraDto?> FindByEmailAsync(string email);
    Task<bool> CheckUserExistsAsync(string userId);
    Task<IList<string>> GetRolesAsync(UserInfraDto user);
    Task<UserInfraDto?> FindByIdAsync(string userId);
    Task RemoveFromRoleAsync(UserInfraDto user, string role);
    Task<IdentityResult> AddToRoleAsync(UserInfraDto user, string role);
}
