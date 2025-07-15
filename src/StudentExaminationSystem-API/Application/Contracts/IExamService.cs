using Application.Common.ErrorAndResults;
using Application.DTOs.ExamDtos;
using Shared.ResourceParameters;

namespace Application.Contracts;

public interface IExamService
{
    Task<Result<PagedList<GetExamHistoryAppDto>>> GetAllAsync(ExamHistoryResourceParameters resourceParameters);
    Task<Result<GetFullExamAppDto?>> GetFullExamAsync(int examId);
    Task<Result<LoadExamAppDto>> GetExamAsync(int subjectId);
}
