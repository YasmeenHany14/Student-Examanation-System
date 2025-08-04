using Application.Common.Constants.Errors;
using Application.Common.ErrorAndResults;
using Application.Contracts;
using Application.DTOs.ExamDtos;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace Application.Services;

public class CacheExamService(IMemoryCache cache, ExamTimerService examTimer, IServiceScopeFactory serviceScopeFactory) : ICacheExamService
{
    public event Func<ExamCacheEntryDto?, Task>? ExamEntryExpired;

    public void CacheExamEntry(int examId, int subjectId, string userId, int durationMinutes)
    {
        var cacheKey = $"ExamEntry_{userId}";
        if (!cache.TryGetValue(cacheKey, out _))
        {
            var entry = new ExamCacheEntryDto(examId, subjectId, durationMinutes);
            var timeInMinutes = durationMinutes + 5;
            var expirationToken = new CancellationChangeToken(
                new CancellationTokenSource(TimeSpan.FromMinutes(timeInMinutes + .01)).Token);
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(timeInMinutes),
                Priority = CacheItemPriority.High,
                ExpirationTokens = { expirationToken }
            };
            
            cacheEntryOptions.PostEvictionCallbacks.Add(new PostEvictionCallbackRegistration
            {
                EvictionCallback = (key, value, reason, state) =>
                {
                    if (reason == EvictionReason.TokenExpired || reason == EvictionReason.Expired)
                    {
                        _ = Task.Run(async () =>
                        {
                            using var scope = serviceScopeFactory.CreateScope();
                            var examService = scope.ServiceProvider.GetRequiredService<IExamService>();
                            await examService.OnExamEntryExpired(entry);
                        });
                    }
                }
            });

            cache.Set(cacheKey, entry, cacheEntryOptions);
            
            // TODO: Failed attempts, will come back to try them again later
            // examTimer.StartTimer(entry, 1);
            //
            // Task.Delay(TimeSpan.FromMinutes(1)).ContinueWith(_ =>
            // {
            //     if (ExamEntryExpired != null)
            //     {
            //         ExamEntryExpired.Invoke(entry);
            //     }
            // });
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
