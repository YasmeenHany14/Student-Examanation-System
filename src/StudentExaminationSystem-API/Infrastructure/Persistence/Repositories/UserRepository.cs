using Domain.Models;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(UserManager<User> userManager) : IUserRepository
{
    public async Task<(string?, IdentityResult)> CreateAsync(User user, string password)
    {
        user.UserName = user.Email;
        var identityResult = await userManager.CreateAsync(user, password);
        return (user.Id, identityResult);
    }

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        var result = await userManager.CheckPasswordAsync(user, password);
        return result;
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<bool> CheckUserExistsAsync(Guid userId)
    {
        return await userManager.FindByIdAsync(userId.ToString()) != null;
    }
    
    public async Task<bool> CheckUserExistsAsync(string email)
    {
        return await userManager.Users.AnyAsync(user => user.Email == email);
    }
    
    public async Task<IList<string>> GetRolesAsync(User user)
    {
        return await userManager.GetRolesAsync(user);
    }

    public async Task RemoveFromRoleAsync(User user, string role)
    {
        await userManager.RemoveFromRoleAsync(user, role);
    }

    public async Task<IdentityResult> AddToRoleAsync(User user, string roleName)
    {
        return await userManager.AddToRoleAsync(user, roleName);
    }

    public async Task<User?> FindByIdAsync(string userId)
    {
        return await userManager.FindByIdAsync(userId);
  
    }
}
