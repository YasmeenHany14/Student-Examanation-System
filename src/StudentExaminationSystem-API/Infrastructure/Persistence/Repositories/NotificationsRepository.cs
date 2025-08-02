using Domain.Models;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.ResourceParameters;

namespace Infrastructure.Persistence.Repositories;

public class NotificationsRepository(
    DataContext context
    ) : BaseRepository<Notification>(context), INotificationsRepository
{
    // Get all notifications for a user
    public async Task<PagedList<Notification>> GetNotificationsByUserIdAsync(
        BaseResourceParameters resourceParameters ,string userId)
    {
        var collection = context.Set<Notification>()
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt);

        return await CreateAsync(
            collection,
            resourceParameters.PageNumber,
            resourceParameters.PageSize);
    }

    public async Task CreateAdminNotificationAsync(string message)
    {
        var adminUserIds = await (from userRole in context.Set<IdentityUserRole<string>>()
                                  join role in context.Set<IdentityRole>() on userRole.RoleId equals role.Id
                                  where role.Name == "Admin"
                                  select userRole.UserId).ToListAsync();

        var notifications = adminUserIds.Select(userId => new Notification(userId, message)).ToList();
        await context.Set<Notification>().AddRangeAsync(notifications);

    }

    public async Task<Notification?> GetByIdAsync(int id)
    {
        return await context.Set<Notification>().FindAsync(id);
    }

    public void UpdateRangeAsync(IEnumerable<Notification> notifications)
    {
        var enumerable = notifications.ToList();
        if (!enumerable.Any())
            return;

        context.Set<Notification>().UpdateRange(enumerable);
    }
}
