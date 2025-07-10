using Domain.DTOs.UserDtos;
using Domain.Models;
using Domain.Repositories;
using Infrastructure.Persistence.Mappers.UserMappers;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(UserManager<User> userManager) : IUserRepository
{
    public async Task<(string?, IdentityResult)> CreateAsync(CreateUserInfraDto infraDto)
    {
        var user = infraDto.ToUser();
        var identityResult = await userManager.CreateAsync(user, infraDto.Password);
        return (user.Id, identityResult);
    }

    public async Task<bool> CheckPasswordAsync(UserInfraDto userInfraDto, string password)
    {
        var user = userInfraDto.ToUser();
        var result = await userManager.CheckPasswordAsync(user, password);
        return result;
    }

    public async Task<bool> CheckPasswordAsync(string email, string password)
    {
        var user = new User { Email = email };
        return await userManager.CheckPasswordAsync(user, password);
    }

    public async Task<UserInfraDto?> FindByEmailAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        return user?.ToUserDto();
    }

    public async Task<bool> CheckUserExistsAsync(string userId)
    {
        return await userManager.FindByIdAsync(userId) != null;
    }

    public async Task<IList<string>> GetRolesAsync(UserInfraDto user)
    {
        var userEntity = user.ToUser();
        return await userManager.GetRolesAsync(userEntity);
    }

    public async Task RemoveFromRoleAsync(UserInfraDto user, string role)
    {
        var userEntity = user.ToUser();
        await userManager.RemoveFromRoleAsync(userEntity, role);
    }

    public async Task<IdentityResult> AddToRoleAsync(UserInfraDto userInfraDto, string roleName)
    {
        var user = userInfraDto.ToUser();
        return await userManager.AddToRoleAsync(user, roleName);
    }

    public async Task<IdentityResult> AddToRoleAsync(string userId, string roleName)
    {
        var user = new User { Id = userId };
        return await userManager.AddToRoleAsync(user, roleName);
    }

    public async Task<UserInfraDto?> FindByIdAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        return user?.ToUserDto();
    }
}
