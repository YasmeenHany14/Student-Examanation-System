using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserExtensionsRepository(DataContext dbContext) : IUserExtensionsRepository
{
    public async Task<IEnumerable<string>> GetAdminUserIdsAsync()
    {
        return await (from userRole in dbContext.Set<IdentityUserRole<string>>()
                      join role in dbContext.Set<IdentityRole>() on userRole.RoleId equals role.Id
                      where role.Name == "Admin"
                      select userRole.UserId).ToListAsync();
    }
}
