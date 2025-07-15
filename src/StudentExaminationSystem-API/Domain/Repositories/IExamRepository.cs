using Domain.DTOs;
using Domain.Models;
using Shared.ResourceParameters;

namespace Domain.Repositories;

public interface IExamRepository : IBaseRepository<GeneratedExam>
{
    Task<PagedList<GetAllExamsInfraDto>> GetAllExamHistoryAsync(
        ExamHistoryResourceParameters resourceParameters, string? userId);
    Task<GetFullExamInfraDto?> GetAllQuestionHistoryAsync(int examId);
}
