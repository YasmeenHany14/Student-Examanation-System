using System.Collections;
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

    public async Task<IEnumerable<Notification>> GetAllUnreadNotificationsAsync(string userId)
    {
        return await context.Set<Notification>()
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync();
    }

    public async Task<IEnumerable<Notification>> CreateRangeAsync(IEnumerable<Notification> notifications)
    {
        var enumerable = notifications.ToList();
        await context.Set<Notification>().AddRangeAsync(enumerable);
        return enumerable;
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
