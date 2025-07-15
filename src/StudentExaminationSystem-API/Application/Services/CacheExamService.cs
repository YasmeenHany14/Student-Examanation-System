using Application.Common.Constants.Errors;
using Application.Common.ErrorAndResults;
using Application.Contracts;
using Application.DTOs.ExamDtos;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services;

public class CacheExamService(IMemoryCache cache) : ICacheExamService
{
    public void CacheExamEntry(int examId, int subjectId, string userId, int durationMinutes)
    {
        var cacheKey = $"ExamEntry_{userId}";
        if (!cache.TryGetValue(cacheKey, out _))
        {
            var entry = new ExamCacheEntryDto(examId, subjectId, durationMinutes);
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(durationMinutes + 5)
            };
            cache.Set(cacheKey, entry, cacheEntryOptions);
        }
    }
    
    public void RemoveExamEntry(string userId)
    {
        var cacheKey = $"ExamEntry_{userId}";
        if (cache.TryGetValue(cacheKey, out _))
        {
            cache.Remove(cacheKey);
        }
    }
    
    public Result<ExamCacheEntryDto?> GetExamEntryAsync(string userId)
    {
        var cacheKey = $"ExamEntry_{userId}";
        if (cache.TryGetValue(cacheKey, out ExamCacheEntryDto? entry))
        {
            return Result<ExamCacheEntryDto?>.Success(entry);
        }
        return Result<ExamCacheEntryDto?>.Failure(CommonErrors.NotFound());
    }
}
