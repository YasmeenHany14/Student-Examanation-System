using Application.Common.ErrorAndResults;
using Application.Contracts;
using Application.DTOs;
using Application.Mappers;
using Domain.DTOs;
using Domain.Repositories;

namespace Application.Services;

public class DashboardService(IUnitOfWork unitOfWork) : IDashboardService
{
    public async Task<Result<AdminDashboardAppDto>> GetAdminDashboardAppAsync()
    {
        var examData = await unitOfWork.ExamHistoryRepository.GetDashboardDataAsync();
        var studentCount = await unitOfWork.StudentRepository.GetTotalStudentsCountAsync();
        examData.TotalUsers = studentCount;
        return Result<AdminDashboardAppDto>.Success(examData.MapTo<AdminDashboardInfraDto, AdminDashboardAppDto>());
    }
}
