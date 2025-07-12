using Application.Contracts;
using Application.DTOs.StudentDtos;
using Application.Services;
using FluentValidation;

namespace Application;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IGenerateTokenService, GenerateTokenService>();
        services.AddValidatorsFromAssemblyContaining<CreateStudentAppDto>();
        return services;
    }
}
