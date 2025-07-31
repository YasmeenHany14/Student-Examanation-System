using System.Security.Claims;
using Application;
using Application.Common.Constants.Notifications;
using Domain.Models;
using Infrastructure;
using Infrastructure.Persistence.SeedData;
using Microsoft.AspNetCore.Identity;
using WebApi;
using WebApi.Helpers;
using WebApi.Helpers.Filters;
using WebApi.Hubs;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ValidationFilter());
    options.InputFormatters.Insert(0, JsonPatchInputFormatter.GetJsonPatchInputFormatter());
});
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddWebApi(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAntiforgery();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(IdentityData.AdminUserPolicyName, p =>
    {
        p.RequireClaim(ClaimTypes.Role, "Admin");
    });
    options.AddPolicy(IdentityData.OwnerUserPolicyName, policy => policy.RequireRole("User"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost4200",
        policyBuilder => policyBuilder.WithOrigins("http://localhost:4200") // Allow localhost:4200
            .AllowAnyHeader() // Allow any headers
            .AllowAnyMethod()); // Allow any methods
});

var app = builder.Build();

#region Seed Data
if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using var scope = scopedFactory.CreateScope();
    var service = scope.ServiceProvider.GetService<Seed>();
    var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
    service.SeedDataContext(userManager);
}
#endregion


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLocalhost4200");
app.UseExceptionHandler();
app.UseAntiforgery();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.MapHub<NotificationsHub>("/hubs/notifications");

app.Run();
