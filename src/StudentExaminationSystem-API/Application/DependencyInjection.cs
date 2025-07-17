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
        services.AddMemoryCache();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IGenerateTokenService, GenerateTokenService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<ISubjectService, SubjectService>();
        services.AddScoped<ISubjectExamConfigService, SubjectExamConfigService>();
        services.AddScoped<IDifficultyProfileService, DifficultyProfileService>();
        services.AddScoped<IExamService, ExamService>();
        services.AddScoped<ICacheExamService, CacheExamService>();
        services.AddScoped<IGenerateExamService, GenerateExamService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IUserService, UserService>();
        services.AddValidatorsFromAssemblyContaining<CreateStudentAppDto>();
        return services;
    }
}
