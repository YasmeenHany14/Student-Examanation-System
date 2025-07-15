using Application.Common.ErrorAndResults;
using Application.DTOs.ExamDtos;

namespace Application.Contracts;

public interface ICacheExamService
{
    void CacheExamEntry(int examId, int subjectId, string userId, int durationMinutes);
    void RemoveExamEntry(string userId);
    Result<ExamCacheEntryDto?> GetExamEntryAsync(string userId);
}
