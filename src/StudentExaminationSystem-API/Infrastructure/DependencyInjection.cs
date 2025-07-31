using Domain.Interfaces;
using Domain.Models;
using Domain.Repositories;
using Domain.UserContext;
using Infrastructure.Helpers;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Persistence.Repositories;
using Infrastructure.RabbitMQ;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<UpdateAuditFieldsInterceptor>();
        services.AddDbContext<DataContext>((sp, options) =>
        {
            var auditInterceptor = sp.GetRequiredService<UpdateAuditFieldsInterceptor>();
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            options.AddInterceptors(auditInterceptor);
            options.EnableDetailedErrors();
        });
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<ISubjectRepository, SubjectRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<ISubjectExamConfigRepository, SubjectExamConfigRepository>();
        services.AddScoped<IDifficultyProfileRepository, DifficultyProfileRepository>();
        services.AddScoped<IExamRepository, ExamRepository>();
        services.AddScoped<IPublisher, Publisher>();
        
        services.AddSingleton<IUserContext, UserContext>();
        services.AddIdentity<User, IdentityRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;

                // User settings
                options.User.RequireUniqueEmail = true; 
            }).AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();
        return services;
    }
}
