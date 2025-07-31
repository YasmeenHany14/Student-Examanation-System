using Application.Common.ErrorAndResults;
using Application.DTOs;
using Application.DTOs.ExamDtos;
using Shared.ResourceParameters;

namespace Application.Contracts;

public interface IExamService
{
    Task<Result<PagedList<GetExamHistoryAppDto>>> GetAllAsync(ExamHistoryResourceParameters resourceParameters);
    Task<Result<GetFullExamAppDto?>> GetFullExamAsync(int examId);
    Task<Result<LoadExamAppDto>> GetExamAsync(int subjectId);
    Task<Result<bool>> SubmitExamAsync(LoadExamAppDto ExamDto);
    Task<Result<(int, int)>> SaveExamEvaluationAsync(ExamEvaluationDto examEvaluationDto);
}
