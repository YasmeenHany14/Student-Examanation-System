using EvaluationService.Application.Interfaces;
using EvaluationService.Application.Services;
using EvaluationService.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EvaluationService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IConsumer, Consumer>();
        services.AddScoped<IEvaluateExamService, EvaluateExamService>();
        services.AddScoped<IMessageProcessingService, MessageProcessingService>();
        services.AddHostedService<Worker>();
        return services;
    }
}
