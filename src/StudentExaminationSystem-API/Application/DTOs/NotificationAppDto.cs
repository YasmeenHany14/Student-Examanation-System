namespace Application.DTOs;

public class NotificationAppDto : AppBaseDto
{
    public string Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime TimeStamp { get; set; }
}
