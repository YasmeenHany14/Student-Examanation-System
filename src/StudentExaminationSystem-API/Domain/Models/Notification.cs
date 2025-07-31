using Domain.Models.Common;

namespace Domain.Models;

public class Notification : PrimaryKeyBaseEntity, IAuditDateOnly
{
    public string UserId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;

    public Notification() { }

    public Notification(string userId, string message)
    {
        UserId = userId;
        Message = message;
    }
}
