using Application.Contracts;
using Microsoft.Extensions.Hosting;

namespace Application.Helpers;

public class Worker(IConsumer consumer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await consumer.ConsumeAsync();
    }
}