using EvaluationService.Application;
using EvaluationService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure();
builder.Services.AddApplicationServices();

var app = builder.Build();


app.Run();
