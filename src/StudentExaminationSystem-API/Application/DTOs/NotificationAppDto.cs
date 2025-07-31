namespace Application.DTOs;

public class NotificationAppDto : AppBaseDto
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
