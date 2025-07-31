using EvaluationService.Application.Interfaces;
using Microsoft.Extensions.Hosting;

namespace EvaluationService.Application;

public class Worker(IConsumer consumer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await consumer.ConsumeAsync();
    }
}
