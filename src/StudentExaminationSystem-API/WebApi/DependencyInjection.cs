using System.Security.Claims;
using System.Text;
using Application.Contracts;
using Application.DTOs.DifficultyProfileDtos;
using Application.DTOs.ExamDtos;
using Application.DTOs.QuestionDtos;
using Application.DTOs.StudentDtos;
using Application.DTOs.SubjectsDtos;
using Infrastructure.Persistence.SeedData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shared.ResourceParameters;
using WebApi.Helpers.ExceptionHandlers;
using WebApi.Helpers.Filters;
using WebApi.Helpers.PaginationHelper;
using WebApi.Services;

namespace WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddTransient<Seed>();
        services.AddScoped<IPaginationHelper<GetStudentByIdAppDto, StudentResourceParameters>
            , PaginationHelper<GetStudentByIdAppDto, StudentResourceParameters>>();
        services.AddScoped<IPaginationHelper<GetSubjectAppDto, SubjectResourceParameters>
            , PaginationHelper<GetSubjectAppDto, SubjectResourceParameters>>();
        services.AddScoped<IPaginationHelper<GetQuestionAppDto, QuestionResourceParameters>
            , PaginationHelper<GetQuestionAppDto, QuestionResourceParameters>>();
        services.AddScoped<IPaginationHelper<GetDifficultyProfileAppDto, DifficultyProfileResourceParameters>
            , PaginationHelper<GetDifficultyProfileAppDto, DifficultyProfileResourceParameters>>();
        services.AddScoped<IPaginationHelper<GetExamHistoryAppDto, ExamHistoryResourceParameters>
            , PaginationHelper<GetExamHistoryAppDto, ExamHistoryResourceParameters>>();
        services.AddSignalR();
        services.AddScoped<INotificationsHub, NotificationsHubWrapper>();
        
        services.AddProblemDetails();
        services.AddScoped<CanAccessResourceFilter>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        
        #region Configure JWT Authentication
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = configuration.GetValue<bool>("Jwt:ValidateIssuer"),
                    ValidateAudience = configuration.GetValue<bool>("Jwt:ValidateAudience"),
                    ValidateLifetime = configuration.GetValue<bool>("Jwt:ValidateLifetime"),
                    ValidateIssuerSigningKey = configuration.GetValue<bool>("Jwt:ValidateIssuerSigningKey"),
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    ClockSkew = TimeSpan.Zero,
                    RoleClaimType = ClaimTypes.Role,
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        // Check if the request is for your hub
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/notifications"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        #endregion
        
        return services;
    }
}
