namespace Application.DTOs;

public class AdminDashboardAppDto : AppBaseDto
{
    public int TotalUsers { get; set; }
    public int TotalExamsCompleted { get; set; }
    public int PassedExamsCount { get; set; }
    public int FailedExamsCount { get; set; }
}
