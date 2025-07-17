using Application.Common.ErrorAndResults;
using Application.DTOs;

namespace Application.Contracts;

public interface IDashboardService
{
    Task<Result<AdminDashboardAppDto>> GetAdminDashboardAppAsync();
}