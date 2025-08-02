using Domain.Models;
using Shared.ResourceParameters;

namespace Domain.Repositories;

public interface INotificationsRepository : IBaseRepository<Notification>
{
    Task<PagedList<Notification>> GetNotificationsByUserIdAsync(BaseResourceParameters resourceParameters,
        string userId);
    
    Task CreateAdminNotificationAsync(string message);
    Task<Notification?> GetByIdAsync(int id);
    void UpdateRangeAsync(IEnumerable<Notification> notifications);
}
