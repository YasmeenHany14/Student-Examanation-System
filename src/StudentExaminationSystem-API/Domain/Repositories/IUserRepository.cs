using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task<(string?, IdentityResult)> CreateAsync(User user, string password);
    Task<bool> CheckPasswordAsync(User user, string password);
    Task<User?> FindByEmailAsync(string email);
    Task<bool> CheckUserExistsAsync(string email);
    Task<bool> CheckUserExistsAsync(Guid userId);
    Task<IList<string>> GetRolesAsync(User user);
    Task<User?> FindByIdAsync(string userId);
    Task RemoveFromRoleAsync(User user, string role);
    Task<IdentityResult> AddToRoleAsync(User user, string roleName);
    Task<IdentityResult> UpdateAsync(User user);
}
