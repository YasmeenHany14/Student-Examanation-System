using EvaluationService.Domain.Interfaces;
using EvaluationService.Infrastructure.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;

namespace EvaluationService.Infrastructure;

public static class DependecyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IPublisher, Publisher>();
        return services;
    }
    
}
