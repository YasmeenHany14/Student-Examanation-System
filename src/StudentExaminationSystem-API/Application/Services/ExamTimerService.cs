using System.Collections.Concurrent;
using Application.DTOs.ExamDtos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class ExamTimerService : BackgroundService
{
    private readonly ConcurrentDictionary<int, Timer> _activeTimers = new();
    private readonly ILogger<ExamTimerService> _logger;
    public event Func<ExamCacheEntryDto, Task>? ExamEntryExpired;
    
    public void StartTimer(ExamCacheEntryDto dto, int expiryMinutes)
    {
        if (_activeTimers.TryRemove(dto.ExamId, out var oldTimer))
            oldTimer.Dispose();

        var timer = new Timer(OnTimerElapsed, dto, 
            TimeSpan.FromMinutes(expiryMinutes), 
            Timeout.InfiniteTimeSpan);

        _activeTimers.TryAdd(dto.ExamId, timer);
    }
    
    private void OnTimerElapsed(object? state)
    {
        if (state is not ExamCacheEntryDto dto)
            return;

        if (_activeTimers.TryRemove(dto.ExamId, out var timer))
        {
            timer.Dispose();
            try
            {
                ExamEntryExpired?.Invoke(dto).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ExamEntryExpired handler");
            }
        }
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
        => Task.CompletedTask;
}
