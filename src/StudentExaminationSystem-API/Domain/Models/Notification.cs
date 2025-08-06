using Domain.Models.Common;

namespace Domain.Models;

public class Notification : BaseEntity, IAuditDateOnly
{
    public string Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;

    public Notification() { }

    public Notification(string userId, string message)
    {
        Id = Guid.NewGuid().ToString();
        UserId = userId;
        Message = message;
    }
}
