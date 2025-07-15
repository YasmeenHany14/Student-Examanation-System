using Application.Common.ErrorAndResults;
using Application.DTOs.ExamDtos;

namespace Application.Contracts;

public interface IGenerateExamService
{
    Task<Result<LoadExamAppDto>> GenerateExamAsync(int subjectId, int userId);

    Task<Result<LoadExamAppDto>> GetCachedExamEntryAsync(ExamCacheEntryDto examCacheEntryDto);
}
